using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3.Middleware;

namespace Task3.Views
{
    internal class Menu : IOLogical
    {
        public void MainMenu() 
        {
            string filePath;// Хранит путь к файлику

            while (true)
            {
                Output("Введите путь до файла с данными: ", false);
                filePath = InputLine();

                if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                    break;

                Output("Некорректный путь к файлу. Попробуйте снова.");
            }// Задает путь к файлику

            using (var workbook = new XLWorkbook(filePath))
            {
                var customersSheet = workbook.Worksheet("Клиенты"); // Хранит таблицу клиентов
                var productsSheet = workbook.Worksheet("Товары");   // Хранит таблицу товаров
                var ordersSheet = workbook.Worksheet("Заявки");     // Хранит таблицу заявок

                var separator = "---------------------------------------------------------------"; //Разделитель блоков 

                DataHandler dataHandler = new DataHandler();

                while (true)
                {
                    Output("");
                    Output("Выберите команду:");
                    Output("1. Поиск клиентов по наименованию товара");
                    Output("2. Изменение контактного лица клиента");
                    Output("3. Поиск золотого клиента");
                    Output("4. Выйти");
                    Output(separator);
                    string command = InputLine();

                    switch (command)
                    {
                        case "1":
                            Output("Введите наименование товара:");
                            string productName = InputLine();
                            dataHandler.SearchCustomersByProduct(productsSheet, customersSheet, ordersSheet, productName);
                            Output(separator);
                            break;

                        case "2":
                            Output("Введите название организации клиента:");
                            string customerOrganization = InputLine();
                            Output("Введите ФИО нового контактного лица организации:");
                            string newContactPerson = InputLine();
                            dataHandler.UpdateContactPerson(customersSheet, customerOrganization, newContactPerson);
                            Output(separator);
                            break;

                        case "3":
                            Output("Введите год:");
                            int year;
                            if (!int.TryParse(InputLine(), out year))
                            {
                                Output("Некорректный ввод года. Попробуйте снова.");
                                Output(separator);
                                break;
                            }
                            Output("Введите месяц:");
                            int month;
                            if (!int.TryParse(InputLine(), out month))
                            {
                                Output("Некорректный ввод месяца. Попробуйте снова.");
                                Output(separator);
                                break;
                            }
                            dataHandler.FindGoldenCustomer(ordersSheet, customersSheet, year, month);
                            Output(separator);
                            break;

                        case "4":
                            return;

                        default:
                            Output("Неопознанная команда, попробуйте еще раз");
                            Output(separator);
                            break;
                    }
                }// Главное меню
            }
        }
    }
}
