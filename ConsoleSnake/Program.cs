using System.Drawing;

namespace ConsoleSnake
{
    internal class Program
    {
        const ConsoleColor BACKGROUND_COLOUR_1 = ConsoleColor.Green;
        const ConsoleColor BACKGROUND_COLOUR_2 = ConsoleColor.DarkGreen;

        static void Main(string[] args)
        {
            Start();
        }

        static void Start()
        {
            Console.CursorVisible = false;

            int SnakeSpeed = 70;
            Size gridDimensions = new Size(12,12);

            List<Point> snakeCoords = new();
            List<byte> moveList = new();
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
            

            moveList.Add(1);
            moveList.Add(1);
            snakeCoords.Add(new Point(0, gridDimensions.Height / 2 - 1));
            snakeCoords.Add(new Point(1, gridDimensions.Height / 2 - 1));
            snakeCoords.Add(new Point(2, gridDimensions.Height / 2 - 1));

            fruitCoords = newFruitCoords(gridDimensions);

            snakeMoveTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs e) => SnakeMove();

            WriteInitialGrid(gridDimensions);
        }

        static void WriteInitialGrid(Size gridDimensions)
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