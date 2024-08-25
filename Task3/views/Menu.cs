using System;

namespace Task3.views
{
    public class Menu
    {
        public Menu() 
        {
            while (true)
            {
                IOLogic.Output("Выберите команду:");
                IOLogic.Output("1. Информация о клиентах заказавших товар");
                IOLogic.Output("2. Изменение контактного лица клиента");
                IOLogic.Output("3. Определить золотого клиента");
                IOLogic.Output("4. Выход");
                IOLogic.Output("host@Admin:~$ ", false);
                string command = IOLogic.Input();                

                switch (command)
                {
                    case "1":
                        //ViewProductOrders();
                        break;
                    case "2":
                        //ChangeContactPerson();
                        break;
                    case "3":
                        //FindGoldenClient();
                        break;
                    case "4":
                        return;
                    default:
                        IOLogic.Output("Неопознанная команда, попробуйте еще раз");
                        break;
                }
            }
        }
    }
}
