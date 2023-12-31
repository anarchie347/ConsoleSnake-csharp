﻿using System.Drawing;
using System.Net;
using static System.Net.WebRequestMethods;

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

			Start(args);
		}

		private static void Start(string[] args)
		{
			Random r = new Random();
			if (args.Contains("-h") || args.Contains("?") || args.Contains("--help"))
			{
				HelpMenu();
				return;
			}
			ColourOptions colours = GetColours(args);
			Options options = GetOptions(args);

			if (!CheckAppleCountGridSize(options))
			{
				Console.WriteLine($"Error, cannot have {options.FruitCount} fruit in a {options.GridWidth}x{options.GridHeight} grid, ther eis not enough room");
				return;
			}
			CheckConsoleSize(options.GridWidth, options.GridHeight, options.BasicScore);
			ClearConsoleSpace(Grid.SQUARE_HEIGHT * options.GridHeight + 3);
			//int fruitCount = Math.Max(1, ParseParameter(args, "fruitcount", 1));
			//int speed = 1000 / Math.Max(1, ParseParameter(args, "speed", 7));

			_ = new Game(options, colours);
		}
		private static ColourOptions GetColours(string[] args)
		{
			return new ColourOptions
			{
				SnakeHead = ApplyEnum<Colour>(ParseParameter(args, "snakeheadcolour", "DarkBlue")),
				SnakeBody = ApplyEnum<Colour>(ParseParameter(args, "snakebodycolour", "Blue")),
				Background1 = ApplyEnum<Colour>(ParseParameter(args, "background1colour", "Green")),
				Background2 = ApplyEnum<Colour>(ParseParameter(args, "background2colour", "DarkGreen")),
				Fruit = ApplyEnum<Colour>(ParseParameter(args, "fruitcolour", "Red"))
            };
		}
		private static Options GetOptions(string[] args)
		{
			if (CheckForFlag(args, "random")) {
				Random r = new();

				int gridWidth = r.Next(5, 31);
				int gridHeight = r.Next(5, 31);

				return new Options()
                {
                    BasicScore = CheckForFlag(args, "basicscore"),
                    QuickExit = CheckForFlag(args, "quickexit"),
                    Pacifist = r.Next(0,2) == 1,
                    Muted = CheckForFlag(args, "mute"),
                    Cheese = r.Next(0, 2) == 1,
                    Debug = CheckForFlag(args, "debug"),


                    FruitCount = r.Next(1, gridWidth * gridHeight - 5),
                    Speed = r.Next(1,101),
                    GridWidth = gridWidth,
                    GridHeight = gridHeight
                };
            } else
			{
                return new Options()
                {
                    BasicScore = CheckForFlag(args, "basicscore"),
                    QuickExit = CheckForFlag(args, "quickexit"),
                    Pacifist = CheckForFlag(args, "pacifist"),
                    Muted = CheckForFlag(args, "mute"),
                    Cheese = CheckForFlag(args, "cheese"),
                    Debug = CheckForFlag(args, "debug"),


                    FruitCount = ApplyBounds(ParseParameter(args, "fruitcount", 1), 1, 140),
                    Speed = ApplyBounds(ParseParameter(args, "speed", 7), 1, 100),
                    GridWidth = ApplyBounds(ParseParameter(args, "gridwidth", 12), 5, 30),
                    GridHeight = ApplyBounds(ParseParameter(args, "gridheight", 12), 5, 30)
                };
            }
            
        }

		private static bool CheckAppleCountGridSize(Options options)
		{
			int gridArea = options.GridHeight * options.GridWidth;
			int startSnakeSize = 4;
			gridArea -= startSnakeSize;
			if (gridArea <= options.FruitCount)
			{
				return false;
			}
			return true;
		}
		private static void CheckConsoleSize(int gridWidth, int gridHeight, bool basicScoring)
		{
			int minWidth;
			if (basicScoring)
				minWidth = Grid.SQUARE_WIDTH * gridWidth;
			else
				minWidth = Grid.SQUARE_WIDTH * (gridWidth + 2) + 24;
			int minHeight = Grid.SQUARE_HEIGHT * gridHeight + 3;

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

		public static void Exit(int endX, int endY, bool quickExit)
		{
			Console.ResetColor();
			if (quickExit)
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

		public static int ApplyBounds(int number, int? min = null, int? max = null)
		{
			if (min != null && number < min)
				return min.Value;
			if (max != null && number > max)
				return max.Value;
			return number;
		}

		public static T ApplyEnum<T>(string value)
		{
			if (Enum.TryParse(typeof(T), value, out object? result))
			{
				if (result == null)
					throw new ArgumentException($"{value} was not a member of enum {typeof(T)} because it was null");
				return (T)result;
			}
			throw new ArgumentException($"{value} was not a member of enum {typeof(T)}");
		}

		public static bool CheckForFlag(string[] args, string paramName, bool allowShort = true)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "--" + paramName || (allowShort && args[i] == "-" + paramName[0]))
				{
					return true;
				}
			}
			return false;
		}
		public static T ParseParameter<T>(string[] args, string paramName, T defaultValue = default)
		{
			T result = defaultValue;
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
					}
					catch
					{
						throw new ArgumentException($"{valueStr} was not of type {typeof(T)}, which was required by the --{paramName} parameter");
					}
				}
			}
			return result;
		}

		private static void HelpMenu()
		{
			Console.ForegroundColor = SNAKE_BODY_COLOUR;
			PrintLogo();
			Console.WriteLine();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Github: https://github.com/anarchie347/BrainfuckPlus");
			Console.WriteLine("Issues: https://github.com/anarchie347/BrainfuckPlus/issues");
			Console.WriteLine("More information: https://github.com/anarchie347/BrainfuckPlus/blob/master/README.md");
			Console.WriteLine();
			Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Flags:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Flag                       | Explanation");
            Console.WriteLine("---------------------------+------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--quickexit,  -q           | Exits immediately and clears the console buffer when the program ends");
            Console.WriteLine("--basicscore,  -b          | Use a more basic score output, useful if the console cannot update fast enough for the normal score output");
            Console.WriteLine("--pacifist,  -p            | The snake is invulnerable");
            Console.WriteLine("--mute,  -m                | Mutes sound effects");
            Console.WriteLine("--cheese,  -c              | The snake has holes in it");
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Value Parameters:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Parameter                  | Value type  | Explanation");
            Console.WriteLine("---------------------------+-------------+------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--fruitcount=value         | integer     | Sets the amount of fruit available at once");
            Console.WriteLine("--speed=value              | integer     | Sets the speed of the snake in tiles/second");
            Console.WriteLine("--gridheight=value         | integer     | Sets the height of the grid");
            Console.WriteLine("--gridwidth=value          | integer     | Sets the width ohe grid");

        }

		private static void PrintLogo()
		{
            Console.WriteLine("                     ,--.                      ,--.          ");
            Console.WriteLine("  .--.--.          ,--.'|   ,---,          ,--/  /|    ,---,.");
            Console.WriteLine(" /  /    '.    ,--,:  : |  '  .' \\      ,---,': / '  ,'  .' |");
            Console.WriteLine("|  :  /`. / ,`--.'`|  ' : /  ;    '.    :   : '/ / ,---.'   |");
            Console.WriteLine(";  |  |--`  |   :  :  | |:  :       \\   |   '   ,  |   |   .'");
            Console.WriteLine("|  :  ;_    :   |   \\ | ::  |   /\\   \\  '   |  /   :   :  |-,");
            Console.WriteLine(" \\  \\    `. |   : '  '; ||  :  ' ;.   : |   ;  ;   :   |  ;/|");
            Console.WriteLine("  `----.   \'   ' ;.    ;|  |  ;/  \\   \\:   '   \\  |   :   .' ");
            Console.WriteLine("  __ \\  \\  ||   | | \\   |'  :  | \\  \\ ,'|   |    ' |   |  |-,");
            Console.WriteLine(" /  /`--'  /'   : |  ; .'|  |  '  '--'  '   : |.  \'   :  ;/| ");
            Console.WriteLine("'--'.     / |   | '`--'  |  :  :        |   | '_\\.'|   |    \\");
            Console.WriteLine("  `--'---'  '   : |      |  | ,'        '   : |    |   :   .'");
            Console.WriteLine("            ;   |.'      `--''          ;   |,'    |   | ,'  ");
            Console.WriteLine("            '---'                       '---'      `----'    ");
        }
	}

	internal enum Direction { Up, Right, Down, Left}
	internal enum Corner { TopRight, BottomRight, BottomLeft, TopLeft}


		//	static void StartOld()
		//	{
		//		Console.CursorVisible = false;

		//		int SnakeSpeed = 70;
		//		Size gridDimensions = new Size(12,12);

		//		List<Point> snakeCoords = new();
		//		List<Direction> moveList = new();
		//		Point fruitCoords;
		//		System.Timers.Timer snakeMoveTimer = new()
		//		{
		//			Interval = SnakeSpeed,
		//			AutoReset = true,
		//			Enabled = false
		//		};
		//		Direction direction = Direction.Right;

		//		string FaceTop = "    ";
		//		string FaceBottom = "    ";


		//		moveList.Add(Direction.Right);
		//		moveList.Add(Direction.Right);
		//		snakeCoords.Add(new Point(0, gridDimensions.Height / 2 - 1));
		//		snakeCoords.Add(new Point(1, gridDimensions.Height / 2 - 1));
		//		snakeCoords.Add(new Point(2, gridDimensions.Height / 2 - 1));

		//		fruitCoords = newFruitCoords(gridDimensions);

		//		Point p;
		//		//snakeMoveTimer.Elapsed += (object? sender, System.Timers.ElapsedEventArgs e) => initialSnakeCoords = SnakeMove(moveList, direction);

		//		OutputInitialGrid(gridDimensions);
		//		OutputInitialSnake(snakeCoords, ref FaceTop, ref FaceBottom);
		//		UpdateGrid(moveList, snakeCoords, ref FaceTop, ref FaceBottom);
		//	}

		//	static void OutputInitialGrid(Size gridDimensions)
		//	{
		//		for (int i = 0; i < gridDimensions.Height; i++)
		//		{
		//			for (int j = 0; j < 2; j++)
		//			{

		//				for (int k = 0; k < gridDimensions.Width; k++)
		//				{
		//					if (i % 2 - k % 2 == 0)
		//						Console.BackgroundColor = BACKGROUND_COLOUR_1;
		//					else
		//						Console.BackgroundColor = BACKGROUND_COLOUR_2;
		//					Console.Write("    ");
		//				}
		//				Console.WriteLine();
		//			}
		//		}
		//	}

		//	static void OutputInitialSnake(List<Point> snakeCoords, ref string faceTop, ref string faceBottom)
		//	{
		//		UpdateFaceValues(Direction.Right, ref faceTop, ref faceBottom);
		//		Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2);
		//		Console.BackgroundColor = SnakeHeadColour;
		//		Console.WriteLine(faceTop);
		//		Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2 + 1);
		//		Console.BackgroundColor = SnakeHeadColour;
		//		Console.WriteLine(faceBottom);


		//		Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 3].X * 4, snakeCoords[snakeCoords.Count - 3].Y * 2);
		//		Console.BackgroundColor = SnakeBodyColour;
		//		Console.WriteLine("    ");
		//		Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 3].X * 4, snakeCoords[snakeCoords.Count - 3].Y * 2 + 1);
		//		Console.BackgroundColor = SnakeBodyColour;
		//		Console.WriteLine("    ");

		//		Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2);
		//		Console.BackgroundColor = SnakeBodyColour;
		//		Console.WriteLine("    ");
		//		Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2 + 1);
		//		Console.BackgroundColor = SnakeBodyColour;
		//		Console.WriteLine("    ");
		//	}

		//	static void UpdateGrid(List<Direction> moveList, List<Point> snakeCoords, ref string faceTop, ref string faceBottom)
		//	{
		//		UpdateFaceValues(moveList.Last(), ref faceTop, ref faceBottom);

		//		Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2);
		//		Console.BackgroundColor = SnakeHeadColour;
		//		Console.WriteLine(faceTop);
		//		Console.SetCursorPosition(snakeCoords.Last().X * 4, snakeCoords.Last().Y * 2 + 1);
		//		Console.BackgroundColor = SnakeHeadColour;
		//		Console.WriteLine(faceBottom);

		//		Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2);
		//		Console.BackgroundColor = SnakeBodyColour;
		//		Console.WriteLine("    ");
		//		Console.SetCursorPosition(snakeCoords[snakeCoords.Count - 2].X * 4, snakeCoords[snakeCoords.Count - 2].Y * 2 + 1);
		//		Console.BackgroundColor = SnakeBodyColour;
		//		Console.WriteLine("    ");


		//	}

		//	static void UpdateFaceValues(Direction lastMove, ref string faceTop, ref string faceBottom)
		//	{
		//		switch (lastMove)
		//		{
		//			case Direction.Up:
		//				if (faceTop == "    ")
		//				{
		//					faceTop = faceBottom;
		//					faceBottom = "    ";
		//				}
		//				break;
		//			case Direction.Right:
		//				if (faceBottom != "    ")
		//					faceBottom = " ಠ_ಠ";
		//				else
		//					faceTop = " ಠ_ಠ";
		//				break;
		//			case Direction.Down:
		//				if (faceBottom == "    ")
		//				{
		//					faceBottom = faceTop;
		//					faceTop = "    ";
		//				}
		//				break;
		//			case Direction.Left:
		//				if (faceBottom != "    ")
		//					faceBottom = "ಠ_ಠ ";
		//				else
		//					faceTop = "ಠ_ಠ ";
		//				break;
		//		}
		//	} 

		//	static void SnakeMove(List<Direction> moveList, Direction direction, List<Point> snakeCoords, int snakeLength, Size gridDimensions, ref bool addSnakeSegment)
		//	{
		//		moveList.Add(direction);

		//		Point behindSnake = snakeCoords[0];

		//		for (int i = 0; i < snakeCoords.Count; i++)
		//		{
		//			switch (moveList[moveList.Count - snakeLength + i])
		//			{
		//				case Direction.Up:
		//					if (snakeCoords[i].Y == 1)
		//						throw new Exception("died");
		//					snakeCoords[i].Offset(0, -1);
		//					break;
		//				case Direction.Right:
		//                       if (snakeCoords[i].X == gridDimensions.Width - 1)
		//                           throw new Exception("died");
		//                       snakeCoords[i].Offset(1, 0);
		//					break;
		//				case Direction.Down:
		//                       if (snakeCoords[i].Y == gridDimensions.Height - 1)
		//                           throw new Exception("died");
		//                       snakeCoords[i].Offset(0, 1);
		//					break;
		//				case Direction.Left:
		//                       if (snakeCoords[i].X == 1)
		//                           throw new Exception("died");
		//                       snakeCoords[i].Offset(-1, 0);
		//					break;
		//			}
		//		}

		//		if (addSnakeSegment)
		//		{
		//			addSnakeSegment = false;
		//			snakeCoords.Add(snakeCoords.Last());
		//			for (int i = snakeCoords.Count - 2; i > -1; i--)
		//			{
		//				snakeCoords[i + 1] = snakeCoords[i];
		//			}
		//		}

		//	}

		//	static Point newFruitCoords(Size gridDimensions)
		//	{
		//		Random r = new();
		//		int x = r.Next(0, gridDimensions.Width);
		//		int y = r.Next(0, gridDimensions.Height);
		//		return new Point(x, y);
		//	}
		//}
}