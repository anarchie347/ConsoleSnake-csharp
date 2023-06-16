using System.Drawing;

namespace ConsoleSnake
{
    internal class Program
    {
        const ConsoleColor BACKGROUND_COLOUR_1 = ConsoleColor.Green;
        const ConsoleColor BACKGROUND_COLOUR_2 = ConsoleColor.DarkGreen;
        const ConsoleColor SNAKE_HEAD_COLOUR = ConsoleColor.DarkBlue;
        const ConsoleColor SNAKE_BODY_COLOUR = ConsoleColor.Blue;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Start();
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void Start()
        {
            Console.CursorVisible = false;

            int SnakeSpeed = 70;
            Size gridDimensions = new Size(12,12);

            List<Point> snakeCoords = new();
            List<Direction> moveList = new();
            Point fruitCoords;
            System.Timers.Timer snakeMoveTimer = new()
            {
                Interval = SnakeSpeed,
                AutoReset = true,
                Enabled = false
            };
            Direction direction = Direction.Right;

            string FaceTop = "    ";
            string FaceBottom = "    ";
            

            moveList.Add(Direction.Right);
            moveList.Add(Direction.Right);
            snakeCoords.Add(new Point(0, gridDimensions.Height / 2 - 1));
            snakeCoords.Add(new Point(1, gridDimensions.Height / 2 - 1));
            snakeCoords.Add(new Point(2, gridDimensions.Height / 2 - 1));

            fruitCoords = newFruitCoords(gridDimensions);

            snakeMoveTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs e) => SnakeMove();

            OutputInitialGrid(gridDimensions);
            OutputInitialSnake(snakeCoords, ref FaceTop, ref FaceBottom);
            UpdateGrid(moveList, snakeCoords, ref FaceTop, ref FaceBottom);
        }

        static void OutputInitialGrid(Size gridDimensions)
        {
            for (int i = 0; i < gridDimensions.Height; i++)
            {
                for (int j = 0; j < 2; j++)
                {

                    for (int k = 0; k < gridDimensions.Width; k++)
                    {
                        if (i % 2 - k % 2 == 0)
                            Console.BackgroundColor = BACKGROUND_COLOUR_1;
                        else
                            Console.BackgroundColor = BACKGROUND_COLOUR_2;
                        Console.Write("    ");
                    }
                    Console.WriteLine();
                }
            }
        }
        
        static void OutputInitialSnake(List<Point> snakeCoords, ref string faceTop, ref string faceBottom)
        {
            UpdateFaceValues(Direction.Right, ref faceTop, ref faceBottom);
            Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2);
            Console.BackgroundColor = SNAKE_HEAD_COLOUR;
            Console.WriteLine(faceTop);
            Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2 + 1);
            Console.BackgroundColor = SNAKE_HEAD_COLOUR;
            Console.WriteLine(faceBottom);


            Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 3].X * 4, snakeCoords[snakeCoords.Count - 3].Y * 2);
            Console.BackgroundColor = SNAKE_BODY_COLOUR;
            Console.WriteLine("    ");
            Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 3].X * 4, snakeCoords[snakeCoords.Count - 3].Y * 2 + 1);
            Console.BackgroundColor = SNAKE_BODY_COLOUR;
            Console.WriteLine("    ");

            Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2);
            Console.BackgroundColor = SNAKE_BODY_COLOUR;
            Console.WriteLine("    ");
            Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2 + 1);
            Console.BackgroundColor = SNAKE_BODY_COLOUR;
            Console.WriteLine("    ");
        }

        static void UpdateGrid(List<Direction> moveList, List<Point> snakeCoords, ref string faceTop, ref string faceBottom)
        {
            UpdateFaceValues(moveList.Last(), ref faceTop, ref faceBottom);

            Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2);
            Console.BackgroundColor = SNAKE_HEAD_COLOUR;
            Console.WriteLine(faceTop);
            Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2 + 1);
            Console.BackgroundColor = SNAKE_HEAD_COLOUR;
            Console.WriteLine(faceBottom);

            Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2);
            Console.BackgroundColor = SNAKE_BODY_COLOUR;
            Console.WriteLine("    ");
            Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2 + 1);
            Console.BackgroundColor = SNAKE_BODY_COLOUR;
            Console.WriteLine("    ");


        }

        static void UpdateFaceValues(Direction lastMove, ref string faceTop, ref string faceBottom)
        {
            switch (lastMove)
            {
                case Direction.Up:
                    if (faceTop == "    ")
                    {
                        faceTop = faceBottom;
                        faceBottom = "    ";
                    }
                    break;
                case Direction.Right:
                    if (faceBottom != "    ")
                        faceBottom = " ಠ_ಠ";
                    else
                        faceTop = " ಠ_ಠ";
                    break;
                case Direction.Down:
                    if (faceBottom == "    ")
                    {
                        faceBottom = faceTop;
                        faceTop = "    ";
                    }
                    break;
                case Direction.Left:
                    if (faceBottom != "    ")
                        faceBottom = "ಠ_ಠ ";
                    else
                        faceTop = "ಠ_ಠ ";
                    break;
            }
        } 

        static void SnakeMove()
        {
            
        }

        static Point newFruitCoords(Size gridDimensions)
        {
            Random r = new();
            int x = r.Next(0, gridDimensions.Width);
            int y = r.Next(0, gridDimensions.Height);
            return new Point(x, y);
        }
    }

    internal enum Direction { Up, Right, Down, Left}

}