using System;

namespace Task3.views
{
    internal static class IOLogic
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
