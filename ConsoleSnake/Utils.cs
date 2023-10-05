using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    internal static class Utils
    {
        public static ConsoleColor GetConsoleColor(Colour colour, Random r)
        {
            return (ConsoleColor)((int)colour < 16 ? (int)colour : r.Next(0, 16));
        }
    }
}
