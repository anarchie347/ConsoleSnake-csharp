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
        private readonly  string SQUARE_LINE_TEXT = new string(' ', SQUARE_WIDTH);

        public Size Dimensions { get; init; }
        public Point StartPoint { get; init; }
        public Snake? Snake { get; private set; }

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
                        Console.Write(SQUARE_LINE_TEXT);
                    }
                    Console.WriteLine();
                }
            }
        }

        public void AddSnake(Snake snake)
        {
            this.Snake = snake;
            Snake.SnakeMove += (object? sender, SnakeMovedEventArgs e) => UpdateSnake(e.Coords, e.BehindSnake);
        }

        private void UpdateSnake(List<Point> snakeCoords, Point behindSnakeCoords)
        {
            //[0] in list is tail, [count - 1] is head
            Point editPoint = behindSnakeCoords;
            Console.BackgroundColor = (editPoint.X % 2 + editPoint.Y % 2 == 0) ? BACKGROUND_COLOUR_1 : BACKGROUND_COLOUR_2;
            for (int i = 0; i < SQUARE_HEIGHT; i++)
            {
                Console.SetCursorPosition(editPoint.X + (editPoint.X * SQUARE_WIDTH), editPoint.Y + (editPoint.Y * SQUARE_HEIGHT + i));
                Console.WriteLine(SQUARE_LINE_TEXT);
            }

            Console.BackgroundColor = Snake.SNAKE_HEAD_COLOUR;
            editPoint = snakeCoords.Last();
            for (int i = 0; i < SQUARE_HEIGHT; i++)
            {
                Console.SetCursorPosition(StartPoint.X + (editPoint.X * SQUARE_WIDTH), StartPoint.Y + (editPoint.Y * SQUARE_HEIGHT + i));
                Console.WriteLine(SQUARE_LINE_TEXT);
            }

            Console.BackgroundColor = Snake.SNAKE_BODY_COLOUR;
            editPoint = snakeCoords[snakeCoords.Count - 2];
            for (int i = 0; i < SQUARE_HEIGHT; i++)
            {
                Console.SetCursorPosition(StartPoint.X + (editPoint.X * SQUARE_WIDTH), StartPoint.Y + (editPoint.Y * SQUARE_HEIGHT + i));
                Console.WriteLine(SQUARE_LINE_TEXT);
            }
        }
    }
}
