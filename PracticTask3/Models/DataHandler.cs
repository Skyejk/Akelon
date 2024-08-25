using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using PracticTask3.Controllers;
using PracticTask3.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticTask3.Models
{
    internal class DataHandler : IDisposable
    {
        private string FilePath { get; set; }
        private bool IsDisposed { get; set; }

        private readonly XLWorkbook _workbook;

        private readonly IXLWorksheet _productsWorksheet;
        private readonly IXLWorksheet _clientsWorksheet;
        private readonly IXLWorksheet _ordersWorksheet;

        public DataHandler(string filePath)
        {
            FilePath = filePath;
            while (true)
            {
                try
                {
                    string str = "C:\\Users\\MNTR\\source\\repos\\SolutionTestClosedXML\\ProjectAkelon\\Resources\\docs.xlsx";

                    _workbook = new XLWorkbook(str);
                    break;
                }
                catch (Exception ex)
                {
                    LogicIO.Output($"Ошибка при открытии файла: {ex.Message}");
                    LogicIO.Output("Пожалуйста, введите путь к файлу заново:");
                    LogicIO.Output("host@Admin:~$ ", false);
                    FilePath = LogicIO.Input();
                }
            }

            _productsWorksheet = _workbook.Worksheet("Товары");
            _clientsWorksheet = _workbook.Worksheet("Клиенты");
            _ordersWorksheet = _workbook.Worksheet("Заявки");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;
            _workbook?.Dispose();
            IsDisposed = true;
        }

        ~DataHandler() => Dispose(false);

        public void Menu()
        {
            while (true)
            {
                LogicIO.Output("Выберите команду:");
                LogicIO.Output("1. Информация о клиентах заказавших товар");
                LogicIO.Output("2. Изменение контактного лица клиента");
                LogicIO.Output("3. Определить золотого клиента");
                LogicIO.Output("4. Выход");
                LogicIO.Output("host@Admin:~$ ", false);
                string command = LogicIO.Input();

                switch (command)
                {
                    case "1":
                        GetClientInfoByProduct();
                        break;
                    case "2":
                        //ChangeContactPerson();
                        break;
                    case "3":
                        FindGoldenClient(_ordersWorksheet, _clientsWorksheet, year, month);
                        break;
                    case "4":
                        return;
                    default:
                        LogicIO.Output("Неопознанная команда, попробуйте еще раз");
                        break;
                }
            }
        }

        public void GetClientInfoByProduct()// Не работает
        {
            LogicIO.Output("Введите наименование товара: ", false);
            string productName = LogicIO.Input().ToUpper();

            bool isProductFound = false;

            var productsRows = _productsWorksheet.RowsUsed().Skip(1); // Пропуск первой строки с заголовками
            foreach (var productRow in productsRows)
            {
                if (productRow.Cell("B").Value.ToString().ToUpper() == productName)
                {
                    Product product = new Product
                    (
                        int.Parse(productRow.Cell("A").Value.ToString()),
                        productRow.Cell("B").Value.ToString(),
                        productRow.Cell("C").Value.ToString(),
                        int.Parse(productRow.Cell("D").Value.ToString())
                    );

                    List<Order> orders = new List<Order>();

                    var ordersRows = _ordersWorksheet.RowsUsed().Skip(1);
                    foreach (var orderRow in ordersRows)
                    {
                        if (orderRow.Cell("B").Value.ToString() == product.Code.ToString())
                        {
                            Order order = new Order
                            (
                                int.Parse(orderRow.Cell("A").Value.ToString()),
                                int.Parse(orderRow.Cell("B").Value.ToString()),
                                int.Parse(orderRow.Cell("C").Value.ToString()),
                                int.Parse(orderRow.Cell("D").Value.ToString()),
                                int.Parse(orderRow.Cell("E").Value.ToString()),
                                DateOnly.ParseExact(orderRow.Cell("F").Value.ToString(), "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture)
                            );
                            order.Product = product;
                            orders.Add(order);
                        }
                    }

                    foreach (var order in orders)
                    {
                        var clientsRows = _clientsWorksheet.RowsUsed().Skip(1);
                        foreach (var clientRow in clientsRows)
                        {
                            if (clientRow.Cell("A").Value.ToString() == order.ClientCode.ToString())
                            {
                                Client client = new Client
                                (
                                    int.Parse(clientRow.Cell("A").Value.ToString()),
                                    clientRow.Cell("B").Value.ToString(),
                                    clientRow.Cell("C").Value.ToString(),
                                    clientRow.Cell("D").Value.ToString()
                                );
                                order.Client = client;
                                break;
                            }
                        }
                    }

                    LogicIO.Output($"\nКоличество заказов товара {product.Name}: {orders.Count}\n");

                    foreach (var order in orders)
                    {
                        LogicIO.Output($"Клиент: {order.Client?.OrganizationName?? "данные о клиенте не найдены в файле"}");
                        LogicIO.Output($"Количество товара: {order.Quantity} {order.Product.Unit}");
                        LogicIO.Output($"Цена за {order.Product.Unit}: {order.Product.Price}");
                        LogicIO.Output($"Общая цена заказа: {order.Quantity * order.Product.Price}");
                        LogicIO.Output($"Дата заказа: {order.OrderDate}\n");
                    }

                    isProductFound = true;
                    break;
                }
            }

            if (!isProductFound)
                LogicIO.Output("\nТовара с подобным названием в таблице нет\n");
        }

        static void FindGoldenClient(IXLWorksheet ordersSheet, IXLWorksheet customersSheet, int year, int month)
        {
            var orderRows = ordersSheet.RowsUsed().Skip(1);

            var customerOrders = new Dictionary<string, int>();

            foreach (var row in orderRows)
            {
                var orderDate = row.Cell(6).GetDateTime();

                if (orderDate.Year == year && orderDate.Month == month)
                {
                    var customerCode = row.Cell(3).GetString();
                    if (customerOrders.ContainsKey(customerCode))
                    {
                        customerOrders[customerCode]++;
                    }
                    else
                    {
                        customerOrders.Add(customerCode, 1);
                    }
                }
            }

            if (customerOrders.Count > 0)
            {
                var goldenCustomerCode = customerOrders.OrderByDescending(c => c.Value).First().Key;
                var customerRow = customersSheet.RowsUsed().Skip(1)
                .FirstOrDefault(r => r.Cell(1).GetString() == goldenCustomerCode);
                if (customerRow != null)
                {
                    Console.WriteLine($"Золотой клиент за {month}.{year}: {customerRow.Cell(2).GetString()}");
                }
            }
            else
            {
                Console.WriteLine($"Заказов за {month}.{year} не найдено.");
            }
        }
    }
}
