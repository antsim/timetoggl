using System;
using System.Security;
using System.Text;

namespace TimeToggl.Helpers
{
    public class ConsoleHelper
    {
        /// <summary>
        /// Gets the console secure password.
        /// </summary>
        /// <returns></returns>
        public static SecureString GetConsoleSecureInput(string label)
        {
            Console.Write($"{label}: ");

            SecureString pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    //Prevent an exception when you hit backspace with no characters on the array.
                    if (pwd.Length > 0)
                    {
                        pwd.RemoveAt(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    if (!char.IsControl(i.KeyChar))
                    {
                        pwd.AppendChar(i.KeyChar);
                        Console.Write("*");
                    }
                }
            }
            return pwd;
        }

        public static string GetConsoleInput(string label)
        {
            Console.Write($"{label}: ");
            return Console.ReadLine();
        }
    }
}
