using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3.Middleware
{
    public class DataHandler
    {
        public void SearchCustomersByProduct(IXLWorksheet productsSheet, IXLWorksheet customersSheet, IXLWorksheet ordersSheet, string productName)
        {
            var productRows = productsSheet.RowsUsed().Skip(1); // Skip(1) -Пропуск первой строки(с заголовком)
            var customers = new List<string>();

            foreach (var row in productRows)
            {
                if (row.Cell(2).GetString() == productName)
                {
                    var productCode = row.Cell(1).GetString();
                    var orderRows = ordersSheet.RowsUsed().Skip(1);

                    foreach (var orderRow in orderRows)
                    {
                        if (orderRow.Cell(2).GetString() == productCode)
                        {
                            var customerCode = orderRow.Cell(3).GetString();
                            var customerRow = customersSheet.RowsUsed().Skip(1)
                                .FirstOrDefault(r => r.Cell(1).GetString() == customerCode); //Сразу выбираем строку по номеру

                            if (customerRow != null)
                            {
                                var customerName = customerRow.Cell(2).GetString();
                                var orderQuantity = Convert.ToInt32(orderRow.Cell(5).GetString());
                                var orderPrice = row.Cell(4).GetDouble();
                                var orderDate = orderRow.Cell(6).GetDateTime();

                                var customerInfo = $"{customerName}, Количество: {orderQuantity}, Цена: {orderPrice}, Дата заказа: {orderDate.ToShortDateString()}";
                                customers.Add(customerInfo);
                            }
                        }
                    }
                }
            }

            if (customers.Count > 0)
            {
                Console.WriteLine("Клиенты, заказавшие данный товар:");
                foreach (var customerInfo in customers)
                {
                    Console.WriteLine(customerInfo);
                }
            }
            else
            {
                Console.WriteLine("Нет клиентов, заказавших данный товар.");
            }
        }

        public void FindGoldenCustomer(IXLWorksheet ordersSheet, IXLWorksheet customersSheet, int year, int month)
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
