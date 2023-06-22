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
		private Grid Grid;

		private Score Score;

		private bool QuickExit;


        public Game(Options options)
		{
            Grid grid = new(new Size(12, 12), new Point(Console.CursorLeft, Console.CursorTop), options.FruitCount);

            List<Point> initialSnakeCoords = new()
            {
                new Point(0, grid.Dimensions.Height / 2 - 1),
                new Point(1, grid.Dimensions.Height / 2 - 1),
                new Point(2, grid.Dimensions.Height / 2 - 1)
            };
            grid.AddSnake(new Snake(initialSnakeCoords, 1000 / options.Speed));

            

            this.Grid = grid;
			QuickExit = options.QuickExit;
			if (options.BasicScore)
                Score = new Score(true, new Point(grid.StartPoint.X, grid.StartPoint.Y + (grid.Dimensions.Height * Grid.SQUARE_HEIGHT)));
            else
				Score = new Score(false, new Point(grid.StartPoint.X + (grid.Dimensions.Width * Grid.SQUARE_WIDTH) + (2 * Grid.SQUARE_WIDTH), grid.StartPoint.Y + (2 * Grid.SQUARE_HEIGHT)));
			grid.FruitEaten += IncreaseScore;
			grid.SnakeDied += (object? sender, EventArgs e) => EndGame();

            Start();
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
							EndGame();
							break;
						case ConsoleKey.Spacebar:
							if (Grid.IsSnakeFrozen)
								Grid.StartSnake();
							else
								Grid.StopSnake();
							break;
						case ConsoleKey.UpArrow:
						case ConsoleKey.W:
                            Grid.ChangeSnakeDirection(Direction.Up);
							break;
						case ConsoleKey.RightArrow:
						case ConsoleKey.D:
                            Grid.ChangeSnakeDirection(Direction.Right);
							break;
						case ConsoleKey.DownArrow:
						case ConsoleKey.S:
                            Grid.ChangeSnakeDirection(Direction.Down);
							break;
						case ConsoleKey.LeftArrow:
						case ConsoleKey.A:
                            Grid.ChangeSnakeDirection(Direction.Left);
							break;
					}
				}
			}
		}

		private void IncreaseScore(object? sender, EventArgs e)
		{
			Score.Value++;
		}

		private void EndGame()
		{
			Grid.StopSnake();
            Program.Exit(0, Grid.StartPoint.Y + (Grid.SQUARE_HEIGHT * Grid.Dimensions.Height) + (Score.Basic ? 1 : 0), QuickExit);
        }
	}
}
