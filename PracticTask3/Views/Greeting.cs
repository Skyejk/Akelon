using PracticTask3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticTask3.Views
{
    internal class Greeting
    {
        public Greeting()
        {
            LogicIO.Output("Добро пожаловать в приложение Akelon Task 3!");
            LogicIO.Output("Для продолжения работы в приложении введите путь к файлу с данными.");
            LogicIO.Output("host@Admin:~$ ", false);
            string filePath = LogicIO.Input();

            using (DataHandler dataHandler = new DataHandler(filePath))
            {
                dataHandler.Menu();
            }

            LogicIO.Input();
        }
    }
}
