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
			Console.CursorVisible = false;
			CheckConsoleSize(args.Contains("-b"));
            Start(args);
		}

		private static void Start(string[] args)
		{
            ClearConsoleSpace(Grid.SQUARE_HEIGHT * 12 + 3);

			//string? fruitCountStr = args.FirstOrDefault(arg => arg.StartsWith("--fruitcount="))?.Substring(13);
			//int fruitCount = 0;
			//if (fruitCountStr != null && !int.TryParse(fruitCountStr, out fruitCount))
			//	throw new ArgumentException($"'{fruitCountStr}' was not an integer {fruitCount}");
			//if (fruitCount == 0)
			//	fruitCount = 1;
			int fruitCount = Math.Max(1, ParseParameter<int>(args, "fruitcount"));
			int speed = 1000 / Math.Max(1, ParseParameter<int>(args, "speed"));

			Grid grid = new(new Size(12, 12), new Point(Console.CursorLeft, Console.CursorTop), fruitCount);

            List<Point> initialSnakeCoords = new()
            {
                new Point(0, grid.Dimensions.Height / 2 - 1),
                new Point(1, grid.Dimensions.Height / 2 - 1),
                new Point(2, grid.Dimensions.Height / 2 - 1)
            };
            grid.AddSnake(new Snake(initialSnakeCoords, speed));

			Game game = new(grid, args.Contains("-q"), args.Contains("-b"));
			game.Start();
        }

		private static void CheckConsoleSize(bool basicScoring)
		{
            int minWidth;
            if (basicScoring)
                minWidth = Grid.SQUARE_WIDTH * 12;
            else
                minWidth = Grid.SQUARE_WIDTH * 14 + 19;
            int minHeight = Grid.SQUARE_HEIGHT * 12 + 3;

			if (Console.BufferWidth < minWidth)
				throw new Exception($"Console buffer width is not big enough, must be at least {minWidth}, was {Console.BufferWidth}");
            if (Console.WindowWidth < minWidth)
                throw new Exception($"Console window width is not big enough, must be at least {minWidth}, was {Console.WindowWidth}");

            if (Console.BufferWidth < minHeight)
                throw new Exception($"Console buffer height is not big enough, must be at least {minHeight}, was {Console.BufferHeight}");
            if (Console.WindowWidth < minHeight)
                throw new Exception($"Console window height is not big enough, must be at least {minHeight}, was {Console.WindowHeight}");
        }

		public static void ClearConsoleSpace(int height)
		{
			Console.Write(new string('\n', height));
			Console.CursorTop -= height;
        }

		public static void Exit(int endX, int endY, bool quickAndClearExit)
		{
            Console.ResetColor();
			if (quickAndClearExit)
			{
				Console.SetCursorPosition(0, 0);
				Console.Clear();
			}
			else
			{
				Console.SetCursorPosition(endX, endY);
				Console.WriteLine("Game over");
			}
			Environment.Exit(0);
		}

		public static T ParseParameter<T>( string[] args, string paramName)
		{
			T result = default(T);
			string valueStr;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("--" + paramName + "="))
				{
					valueStr = args[i].Substring(paramName.Length + 3);
					try
					{
						result = (T)Convert.ChangeType(valueStr, typeof(T));
						return result;
					} catch
					{
						throw new ArgumentException($"{valueStr} was not of type {typeof(T)}, which was required by the --{paramName} parameter");
					}
				}
			}
			return result;
		}

		static void StartOld()
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

			Point p;
			//snakeMoveTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs e) => initialSnakeCoords = SnakeMove(moveList, direction);

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

		static void SnakeMove(List<Direction> moveList, Direction direction, List<Point> snakeCoords, int snakeLength, Size gridDimensions, ref bool addSnakeSegment)
		{
			moveList.Add(direction);

			Point behindSnake = snakeCoords[0];

			for (int i = 0; i < snakeCoords.Count; i++)
			{
				switch (moveList[moveList.Count - snakeLength + i])
				{
					case Direction.Up:
						if (snakeCoords[i].Y == 1)
							throw new Exception("died");
						snakeCoords[i].Offset(0, -1);
						break;
					case Direction.Right:
                        if (snakeCoords[i].X == gridDimensions.Width - 1)
                            throw new Exception("died");
                        snakeCoords[i].Offset(1, 0);
						break;
					case Direction.Down:
                        if (snakeCoords[i].Y == gridDimensions.Height - 1)
                            throw new Exception("died");
                        snakeCoords[i].Offset(0, 1);
						break;
					case Direction.Left:
                        if (snakeCoords[i].X == 1)
                            throw new Exception("died");
                        snakeCoords[i].Offset(-1, 0);
						break;
				}
			}

			if (addSnakeSegment)
			{
				addSnakeSegment = false;
				snakeCoords.Add(snakeCoords.Last());
				for (int i = snakeCoords.Count - 2; i > -1; i--)
				{
					snakeCoords[i + 1] = snakeCoords[i];
				}
			}

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
	internal enum Corner { TopRight, BottomRight, BottomLeft, TopLeft}

}