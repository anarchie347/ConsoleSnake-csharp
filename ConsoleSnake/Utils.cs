using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    internal static class Utils
    {
        public static bool SetConsoleColourIfStatic(Colour colour)
        {
            if ((int)colour < 16)
            {
                Console.ForegroundColor = (ConsoleColor)(int)colour;
                return true;
            }
            return false;
        }
    }
}
