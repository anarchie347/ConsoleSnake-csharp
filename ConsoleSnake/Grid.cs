using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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
                        Console.BackgroundColor = (i % 2 == k % 2) ? BACKGROUND_COLOUR_1 : BACKGROUND_COLOUR_2;
                        Console.Write(SQUARE_LINE_TEXT);
                    }
                    Console.WriteLine();
                }
            }
        }

        public void AddSnake(Snake snake)
        {
            this.Snake = snake;
            Snake.SnakeMove += (object? sender, EventArgs e) => UpdateSnake((sender as Snake).Coords, (sender as Snake).BehindSnakeCoords, (sender as Snake).FacePosition);
        }

        private void UpdateSnake(ReadOnlyCollection<Point> snakeCoords, Point behindSnakeCoords, Corner facePosition)
        {
            //[0] in list is tail, [count - 1] is head
            Point editPoint = behindSnakeCoords;
            Console.BackgroundColor = (editPoint.X % 2 == editPoint.Y % 2) ? BACKGROUND_COLOUR_1 : BACKGROUND_COLOUR_2;
            for (int i = 0; i < SQUARE_HEIGHT; i++)
            {
                Console.SetCursorPosition(editPoint.X * SQUARE_WIDTH, editPoint.Y * SQUARE_HEIGHT + i);
                Console.WriteLine(SQUARE_LINE_TEXT);
            }

            Console.BackgroundColor = Snake.SNAKE_HEAD_COLOUR;
            editPoint = snakeCoords.Last();
            Console.SetCursorPosition(editPoint.X * SQUARE_WIDTH, editPoint.Y * SQUARE_HEIGHT);
            if (SQUARE_HEIGHT > 1)
            {

                if (facePosition.IsOnSide(Direction.Up))
                    if (facePosition.IsOnSide(Direction.Left))
                        Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
                    else
                        Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
                else
                    Console.Write(SQUARE_LINE_TEXT);
                for (int i = 1; i < SQUARE_HEIGHT - 1; i++)
                {
                    Console.SetCursorPosition(editPoint.X * SQUARE_WIDTH, editPoint.Y * SQUARE_HEIGHT + i);
                    Console.WriteLine(SQUARE_LINE_TEXT);
                }
                Console.SetCursorPosition(editPoint.X * SQUARE_WIDTH, (editPoint.Y + 1) * SQUARE_HEIGHT - 1);
                if (facePosition.IsOnSide(Direction.Down))
                    if (facePosition.IsOnSide(Direction.Left))
                        Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
                    else
                        Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
                else
                    Console.Write(SQUARE_LINE_TEXT);
            }
            else
            {
                if (facePosition.IsOnSide(Direction.Left))
                    Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
                else
                    Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
            }

            Console.BackgroundColor = Snake.SNAKE_BODY_COLOUR;
            editPoint = snakeCoords[snakeCoords.Count - 2];
            for (int i = 0; i < SQUARE_HEIGHT; i++)
            {
                Console.SetCursorPosition(editPoint.X * SQUARE_WIDTH, editPoint.Y * SQUARE_HEIGHT + i);
                Console.WriteLine(SQUARE_LINE_TEXT);
            }
        }
    }
}
