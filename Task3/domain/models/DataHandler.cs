using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Task3.controllers;
using Task3.views;

namespace Task3.models
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
                    _workbook = new XLWorkbook(FilePath);
                    Menu menu = new Menu();
                    break;
                }
                catch (Exception ex)
                {
                    IOLogic.Output($"Ошибка при открытии файла: {ex.Message}");
                    IOLogic.Output("Пожалуйста, введите путь к файлу заново:");
                    IOLogic.Output("host@Admin:~$ ", false);
                    FilePath = IOLogic.Input();
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

        public void GetClientInfoByProduct(string productName, List<Product> products, List<Client> clients, List<Order> orders)
        {
            IOLogic.Output("Введите наименование товара: ", false);
            string productName = IOLogic.Input().ToUpper();

            bool isProductFound = false;

            var productsRows = _productsWorksheet.RowsUsed().Skip(1);
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
                        if (orderRow.Cell("B").Value.ToString() == product.Id.ToString())
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
                            if (clientRow.Cell("A").Value.ToString() == order.ClientId.ToString())
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

                    IOLogic.Output($"\nКоличество заказов товара {product.Name}: {orders.Count}\n");

                    foreach (var order in orders)
                    {
                        IOLogic.Output($"Клиент: {order.Client?.Name ?? "данные о клиенте не найдены в файле"}");
                        IOLogic.Output($"Количество товара: {order.ProductAmount} {order.Product.UnitOfMeasure}");
                        IOLogic.Output($"Цена за {order.Product.UnitOfMeasure}: {order.Product.Price}");
                        IOLogic.Output($"Общая цена заказа: {order.ProductAmount * order.Product.Price}");
                        IOLogic.Output($"Дата заказа: {order.Date}\n");
                    }

                    isProductFound = true;
                    break;
                }
            }

            if (!isProductFound)
                IOLogic.Output("\nТовара с подобным названием в таблице нет\n");
        }
        /*
        public void UpdateContactPerson(string organizationName, string newContactPerson, List<Client> clients)
        {
            IOLogic.Output("Введите наименование организации: ", false);
            string organization = IOLogic.Input().ToUpper();
            IOLogic.Output("Введите ФИО нового контактного лица: ", false);
            string contactPerson = IOLogic.Input();

            bool isClientFound = false;

            var clientsRows = _clientsWorksheet.RowsUsed().Skip(1);
            foreach (var clientRow in clientsRows)
            {
                if (clientRow.Cell("B").Value.ToString().ToUpper().Contains(organization))
                {
                    if (clientRow.Cell("B").Value.ToString().ToUpper() != organization)
                    {
                        IOLogic.Output($"Найдено похожее наименование организации: {clientRow.Cell("B").Value}");
                        IOLogic.Output($"Вы имели в виду ее? Да/Нет: ", false);
                        if (IOLogic.Input().ToUpper() == "ДА")
                            isClientFound = true;
                    }
                    else
                        isClientFound = true;

                    if (isClientFound)
                    {
                        IOLogic.Output($"\nСтарое контактное лицо {clientRow.Cell("B").Value}: {clientRow.Cell("D").Value}");
                        clientRow.Cell("D").Value = contactPerson;
                        IOLogic.Output($"Новое контактное лицо {clientRow.Cell("B").Value}: {clientRow.Cell("D").Value}");
                        _workbook.Save();
                        IOLogic.Output("Изменения сохранены\n");

                        break;
                    }
                }
            }

            if (!isClientFound)
            {
                IOLogic.Output("\nОрганизация с подобным наименованием не найдена в таблице\n");
            }
        }
        */
        /*
        public void GetGoldenClient()
        {
            IOLogic.Output("Будет осуществлен поиск клиента с наибольшим числом заказов за указанный период");
            int desiredYear;
            int desiredMonth;
            bool isCorrectYear = false;
            bool isCorrectMonth = false;

            while (true)
            {
                IOLogic.Output("Введите год: ", false);
                isCorrectYear = int.TryParse(IOLogic.Input, out desiredYear);
                IOLogic.Output("Введите месяц: ", false);
                isCorrectMonth = int.TryParse(IOLogic.Input, out desiredMonth);

                if (isCorrectYear && isCorrectMonth)
                    break;
                else
                    IOLogic.Output("Допущена ошибка во введенных значения искомого года и месяца");
            }

            var clientIdOrdersCount = new Dictionary<int, int>();
            var ordersRows = _ordersWorksheet.RowsUsed().Skip(1);

            foreach (var orderRow in ordersRows)
            {
                DateOnly date = DateOnly.ParseExact(orderRow.Cell("F").Value.ToString(), "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture);

                if (date.Year == desiredYear && date.Month == desiredMonth)
                {
                    int clientId = int.Parse(orderRow.Cell("C").Value.ToString());
                    if (clientIdOrdersCount.ContainsKey(clientId))
                        clientIdOrdersCount[clientId]++;
                    else
                        clientIdOrdersCount.Add(clientId, 1);
                }
            }

            if (clientIdOrdersCount.Count == 0)
            {
                IOLogic.Output("\nВ таблице заказов не удалось найти клиентов по заданным критериям\n");
                return;
            }

            int maxOrdersCount = clientIdOrdersCount.Max(x => x.Value);
            List<int> IdMaxOrdersCount = clientIdOrdersCount.Where(x => x.Value == maxOrdersCount)
                                                            .Select(x => x.Key)
                                                            .ToList();
            List<Client> clientsMaxOrders = new List<Client>();
            var clientsRows = _clientsWorksheet.RowsUsed().Skip(1);

            foreach (var clientRow in clientsRows)
            {
                foreach (var id in IdMaxOrdersCount)
                {
                    if (int.Parse(clientRow.Cell("A").Value.ToString()) == id)
                    {
                        clientsMaxOrders.Add(new Client
                        (
                            int.Parse(clientRow.Cell("A").Value.ToString()),
                            clientRow.Cell("B").Value.ToString(),
                            clientRow.Cell("C").Value.ToString(),
                            clientRow.Cell("D").Value.ToString()
                        ));
                    }
                }
            }

            if (clientsMaxOrders.Count != 0)
            {
                IOLogic.Output($"\nМаксимальное количество заказов: {maxOrdersCount}");
                IOLogic.Output($"Клиентов с максимальным числом заказов: {clientsMaxOrders.Count}\n");

                foreach (var client in clientsMaxOrders)
                {
                    IOLogic.Output($"Наименование организации: {client.Name}");
                    IOLogic.Output($"Адрес: {client.Address}");
                    IOLogic.Output($"Контактное лицо: {client.Person}\n");
                }
            }
            else
                IOLogic.Output("\nВ таблице клиентов не удалось найти клиента, который числится в таблице заявок с максимальным числом заказов\n");
        }

        */
        ~DataHandler() => Dispose(false);
    }
}
