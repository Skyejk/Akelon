using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3.Views
{
    internal class Greeting : IOLogical
    {
        public Greeting()
        {
            Output("Добро пожаловать в приложениен Akelon Task 3!");
            Output("Для продолжения нажмите на любую клавишу.");
            EnterAnyKey();

            Menu menu = new Menu();
            menu.MainMenu();
        }
    }
}
