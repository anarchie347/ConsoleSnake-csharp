﻿using System;
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

		private Options Options;


        public Game(Options options, ColourOptions colours)
		{
			Options = options;
            Grid grid = new(new Size(options.GridWidth, options.GridHeight), new Point(Console.CursorLeft, Console.CursorTop), options.FruitCount, colours);

			List<Point> initialSnakeCoords;

            initialSnakeCoords = new()
            {
				new Point(1, grid.Dimensions.Height / 2 - 1),
                new Point(2, grid.Dimensions.Height / 2 - 1),
                new Point(3, grid.Dimensions.Height / 2 - 1),
                new Point(4, grid.Dimensions.Height / 2 - 1),
            };
            grid.AddSnake(new Snake(initialSnakeCoords, 1000 / options.Speed, options.Cheese, colours.SnakeBody, colours.SnakeHead), options.Pacifist, options.Cheese);

            

            this.Grid = grid;
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
			
			if (!Options.Debug)
			{
                ChangeDirection(Console.ReadKey(true).Key);
                Grid.StartSnake();
            }
			int numberOfDebugMessagesOutput = 0;

			//Grid.StopSnake();
			//keyhandling
			while (true)
			{
				if (Console.KeyAvailable)
				{
					
					ConsoleKey key = Console.ReadKey(true).Key;
                    ChangeDirection(key);
					
                    if (Options.Debug)
                    {
						if (key == ConsoleKey.Backspace)
						{
                            Console.ResetColor();
                            for (int i = 0; i < numberOfDebugMessagesOutput; i++)
							{
                                Console.SetCursorPosition(Grid.StartPoint.X, Grid.StartPoint.Y + Grid.SQUARE_HEIGHT * Grid.Dimensions.Height + (Options.BasicScore ? 1 : 0) + i);
								Console.Write(new string(' ', Console.WindowWidth));
                            }
							numberOfDebugMessagesOutput = 0;
						}
						else
						{
                            Grid.MoveSnakeOnce();
							Console.ResetColor();
                            DebugMessage(key, numberOfDebugMessagesOutput);
                            numberOfDebugMessagesOutput++;
                        }
                    }

                }
			}
		}

		private void ChangeDirection(ConsoleKey key)
		{
            switch (key)
            {
                case ConsoleKey.Escape:
                    EndGame();
                    break;
                case ConsoleKey.Spacebar:
                    if (Grid.IsSnakeFrozen)
                    {
                        Grid.StartSnake();
                    }
                    else
                    {
                        Grid.StopSnake(true);
                        Score.ReOutputScore();
                    }

                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    Grid.ChangeSnakeDirection(Direction.Up, Options.Muted);
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    Grid.ChangeSnakeDirection(Direction.Right, Options.Muted);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    Grid.ChangeSnakeDirection(Direction.Down, Options.Muted);
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    Grid.ChangeSnakeDirection(Direction.Left, Options.Muted);
                    break;
            }
        }

		private void DebugMessage(ConsoleKey lastKey, int numberOfDebugMessagesOutput)
		{
            string coordsAsString = "";
            foreach (Point p in Grid.SnakeCoords.Reverse())
                coordsAsString += $"[{p.X}, {p.Y}] ";
            Console.Title = coordsAsString;
            Console.SetCursorPosition(Grid.StartPoint.X, Grid.StartPoint.Y + Grid.SQUARE_HEIGHT * Grid.Dimensions.Height + (Options.BasicScore ? 1 : 0) + numberOfDebugMessagesOutput);

			char arrow = '-';
			switch (lastKey)
			{
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
					arrow = '↑';
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    arrow = '→';
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    arrow = '↓';
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    arrow = '←';
                    break;
            }
			Console.Write($"Move: {arrow}  Coords: {coordsAsString}");



        }

		private void IncreaseScore(object? sender, EventArgs e)
		{
			Score.Value++;
		}

		private void EndGame()
		{
			Grid.StopSnake(false);
            Program.Exit(0, Grid.StartPoint.Y + (Grid.SQUARE_HEIGHT * Grid.Dimensions.Height) + (Score.Basic ? 1 : 0), Options.QuickExit);
        }
	}
}
