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

		private bool QuickAndClearExit;


        public Game(Grid grid, bool quickAndClearExit, bool BasicScore)
		{
			this.Grid = grid;
			QuickAndClearExit = quickAndClearExit;
			if (BasicScore)
                Score = new Score(true, new Point(grid.StartPoint.X, grid.StartPoint.Y + (grid.Dimensions.Height * Grid.SQUARE_HEIGHT)));
            else
				Score = new Score(false, new Point(grid.StartPoint.X + (grid.Dimensions.Width * Grid.SQUARE_WIDTH) + (2 * Grid.SQUARE_WIDTH), grid.StartPoint.Y + (2 * Grid.SQUARE_HEIGHT)));
			grid.FruitEaten += IncreaseScore;
			grid.SnakeDied += (object? sender, EventArgs e) => EndGame();
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
                            Grid.Snake?.ChangeDirection(Direction.Up);
							break;
						case ConsoleKey.RightArrow:
							Grid.Snake?.ChangeDirection(Direction.Right);
							break;
						case ConsoleKey.DownArrow:
							Grid.Snake?.ChangeDirection(Direction.Down);
							break;
						case ConsoleKey.LeftArrow:
							Grid.Snake?.ChangeDirection(Direction.Left);
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
            Program.Exit(0, Grid.StartPoint.Y + (Grid.SQUARE_HEIGHT * Grid.Dimensions.Height) + (Score.Basic ? 1 : 0), QuickAndClearExit, Grid.Snake);
        }
	}
}
