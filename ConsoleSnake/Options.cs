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

    internal struct ColourOptions
    {
        public Colour SnakeBody { get; init; }
        public Colour SnakeHead { get; init; }
        public Colour Fruit { get; init; }
        public Colour Background1 { get; init; }
        public Colour Background2 { get; init; }
    }

    internal enum Colour
    {
        Black = ConsoleColor.Black,
        DarkBlue = ConsoleColor.DarkBlue,
        DarkGreen = ConsoleColor.DarkGreen,
        DarkCyan = ConsoleColor.DarkCyan,
        DarkRed = ConsoleColor.DarkRed,
        DarkMagenta = ConsoleColor.DarkMagenta,
        DarkYellow = ConsoleColor.DarkYellow,
        Grey = ConsoleColor.Gray,
        DarkGrey = ConsoleColor.DarkGray,
        Blue = ConsoleColor.Blue,
        Green = ConsoleColor.Green,
        Cyan = ConsoleColor.Cyan,
        Red = ConsoleColor.Red,
        Magenta = ConsoleColor.Magenta,
        Yellow = ConsoleColor.Yellow,
        White = ConsoleColor.White,
        Rainbow = 16

    }

}
