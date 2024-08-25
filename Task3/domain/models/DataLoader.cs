using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using Task3.controllers;

namespace Task3.models
{
    internal class DataLoader
    {
        public (List<Product>, List<Client>, List<Order>) LoadData(string filePath)
        {
            var products = new List<Product>();
            var clients = new List<Client>();
            var orders = new List<Order>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var productsSheet = workbook.Worksheet("товары");
                var clientsSheet = workbook.Worksheet("Клиенты");
                var ordersSheet = workbook.Worksheet("Заявки");

                // Загрузка товаров
                foreach (var row in productsSheet.RowsUsed().Skip(1))
                {
                    products.Add(new Product("", "", "", "")
                    {
                        Code = row.Cell(1).GetString(),
                        Name = row.Cell(2).GetString(),
                        Unit = row.Cell(3).GetString(),
                        Price = row.Cell(4).GetString(),
                    });
                }

                // Загрузка клиентов
                foreach (var row in clientsSheet.RowsUsed().Skip(1))
                {
                    clients.Add(new Client("","","","")
                    {
                        Code = row.Cell(1).GetString(),
                        OrganizationName = row.Cell(2).GetString(),
                        Address = row.Cell(3).GetString(),
                        ContactPerson = row.Cell(4).GetString()
                    });
                }

                // Загрузка заявок
                foreach (var row in ordersSheet.RowsUsed().Skip(1))
                {

                    orders.Add(new Order("", "", "", "", "", "")
                    {
                        Code = row.Cell(1).GetString(),
                        ProductCode = row.Cell(2).GetString(),
                        ClientCode = row.Cell(3).GetString(),
                        RequestNumber = row.Cell(4).GetString(),
                        Quantity = row.Cell(5).GetString(),
                        OrderDate = row.Cell(6).GetString()
                    });
                }
            }

            return (products, clients, orders);
        }
    }
}
