using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3.Views
{
    public class IOLogical
    {
        public void Output(string message, bool isNewLine = true)
        {
            if (isNewLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }

        public string InputLine()
        {
            return Console.ReadLine() ?? string.Empty;
        }
        public void EnterAnyKey()
        {
            Console.ReadKey();
        }
    }
}
