using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticTask3.Views
{
    public static class LogicIO
    {
        public static void Output(string message, bool NewLine = true)
        {
            if (NewLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }

        public static string Input()
        {
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
