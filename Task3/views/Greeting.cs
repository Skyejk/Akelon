using Task3.models;

namespace Task3.views
{
    internal class Greeting
    {
        public Greeting() 
        {
            IOLogic.Output("Добро пожаловать в приложение Akelon Task 3!");
            IOLogic.Output("Для продолжения работы в приложении введите путь к файлу с данными.");
            IOLogic.Output("host@Admin:~$ ", false);
            string filePath = IOLogic.Input();

            using (DataHandler dataHandler = new DataHandler(filePath))
            {                
            }
        }
    }
}
