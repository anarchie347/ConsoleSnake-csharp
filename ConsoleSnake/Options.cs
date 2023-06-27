using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    internal readonly struct Options
    {
        public bool QuickExit { get; init; }
        public bool BasicScore { get; init; }
        public bool Pacifist { get; init; }
        public bool Muted { get; init; }
        public bool Cheese { get; init; }
        public bool Debug { get; init; }


        public int Speed { get; init; }
        public int FruitCount { get; init; }
        public int GridWidth { get; init; }
        public int GridHeight { get; init; }
        public ConsoleColor SnakeBodyColour { get; init; }
        public ConsoleColor SnakeHeadColour { get ; init; }

    }
}
