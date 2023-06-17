using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{

    internal class Grid
    {
        public const ConsoleColor BACKGROUND_COLOUR_1 = ConsoleColor.Green;
        public const ConsoleColor BACKGROUND_COLOUR_2 = ConsoleColor.DarkGreen;
        public const int SQUARE_HEIGHT = 2;
        public const int SQUARE_WIDTH = 4;
        private readonly  string SQAURE_LINE_TEXT = new string(' ', SQUARE_WIDTH);

        public Size Dimensions { get; init; }
        public Point StartPoint { get; init; }

        public Grid() { }
        public Grid(Size dimensions, Point startPoint)
        {
            Dimensions = dimensions;
            StartPoint = startPoint;
        }

        public void OutputGrid()
        {
            for (int i = 0; i < Dimensions.Height; i++)
            {
                for (int j = 0; j < SQUARE_HEIGHT; j++)
                {
                    for (int k = 0; k < Dimensions.Width; k++)
                    {
                        if (i % 2 - k % 2 == 0)
                            Console.BackgroundColor = BACKGROUND_COLOUR_1;
                        else
                            Console.BackgroundColor = BACKGROUND_COLOUR_2;
                        Console.Write(SQAURE_LINE_TEXT);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
