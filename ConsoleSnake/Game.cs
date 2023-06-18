using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    internal class Game
    {
        public Grid Grid { get; private set; }

        public Score Score { get; private set; }

        public Game(Grid grid)
        {
            this.Grid = grid;
            Score = new Score(false, new Point(grid.StartPoint.X + (grid.Dimensions.Width * Grid.SQUARE_WIDTH) + (2 * Grid.SQUARE_WIDTH), grid.StartPoint.Y + (2 * Grid.SQUARE_HEIGHT)));
            grid.FruitEaten += IncreaseScore;
        }

        public void Start()
        {
            Grid.OutputGrid();
            Console.ReadKey(true);
            Grid.StartSnake();
            //keyhandling
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Escape:
                            Program.Exit(0, Grid.StartPoint.Y + (Grid.SQUARE_HEIGHT * Grid.Dimensions.Height), false, Grid.Snake);
                            break;
                        case ConsoleKey.UpArrow:
                            if (Grid.Snake != null)
                                Grid.Snake.ChangeDirection(Direction.Up);
                            break;
                        case ConsoleKey.RightArrow:
                            if (Grid.Snake != null)
                                Grid.Snake.ChangeDirection(Direction.Right);
                            break;
                        case ConsoleKey.DownArrow:
                            if (Grid.Snake != null)
                                Grid.Snake.ChangeDirection(Direction.Down);
                            break;
                        case ConsoleKey.LeftArrow:
                            if (Grid.Snake != null)
                                Grid.Snake.ChangeDirection(Direction.Left);
                            break;
                    }
                }
            }
        }

        private void IncreaseScore(object? sender, EventArgs e)
        {
            Score.Value++;
        }
    }
}
