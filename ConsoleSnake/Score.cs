using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
	internal class Score
	{
		public bool Basic { get; private set; }
		public Point StartPoint { get; private set; }

		private const string MESSAGE = "Score:";
		private int value;
		public int Value
		{
			get { return value; }
			set
			{
				if (value > -1)
				{
					this.value = value;
					UpdateScoreVisual();
				}
			}
		}
		public Score(bool basic, Point startPoint)
		{
			Basic = basic;
			StartPoint = startPoint;
			value = 0;
			UpdateScoreVisual();
		}

		private void UpdateScoreVisual()
		{
			Console.SetCursorPosition(StartPoint.X, StartPoint.Y);
			Console.ResetColor();
			if (Basic)
			{
				Console.WriteLine($"{MESSAGE} {value}");
			} else
			{
				DigitAsciiArt.Write(value, true, StartPoint.X, StartPoint.Y);
			}
		}
	}

	internal static class DigitAsciiArt
	{
        //from https://onlineasciitools.com/convert-text-to-ascii-art
        //ogre font, default font options

        public static readonly string[] ONE =
		{
            " _ ",
			"/ |",
			"| |",
			"| |",
			"|_|",
		};
        public const int ONE_WIDTH = 3;
        public static readonly string[] TWO =
        {
            " ____  ",
            "|___ \\ ",
            "  __) |",
            " / __/ ",
            "|_____|",
        };
        public const int TWO_WIDTH = 7;
        public static readonly string[] THREE =
        {
            " _____ ",
            "|___ / ",
            "  |_ \\ ",
            " ___) |",
            "|____/ ",
        };
        public const int THREE_WIDTH = 7;
        public static readonly string[] FOUR =
        {
            " _  _   ",
            "| || |  ",
            "| || |_ ",
            "|__   _|",
            "   |_|  ",
        };
        public const int FOUR_WIDTH = 8;
        public static readonly string[] FIVE =
        {
            " ____  ",
            "| ___| ",
            "|___ \\ ",
            " ___) |",
            "|____/ ",
        };
        public const int FIVE_WIDTH = 7;
        public static readonly string[] SIX =
        {
            "  __   ",
            " / /_  ",
            "| '_ \\ ",
            "| (_) |",
            " \\___/ ",
        };
        public const int SIX_WIDTH = 7;
        public static readonly string[] SEVEN =
        {
            " _____ ",
            "|___  |",
            "   / / ",
            "  / /  ",
            " /_/   ",
        };
        public const int SEVEN_WIDTH = 7;
        public static readonly string[] EIGHT =
        {
            "  ___  ",
            " ( _ ) ",
            " / _ \\ ",
            "| (_) |",
            " \\___/ ",
        };
        public const int EIGHT_WIDTH = 7;
        public static readonly string[] NINE =
        {
            "  ___  ",
            " / _ \\ ",
            "| (_) |",
            " \\__, |",
            "   /_/ ",
        };
        public const int NINE_WIDTH = 7;
        public static readonly string[] ZERO =
        {
            "  ___  ",
            " / _ \\ ",
            "| | | |",
            "| |_| |",
            " \\___/ ",
        };
        public const int ZERO_WIDTH = 7;

        public static readonly string[][] ALL =
        {
            ZERO,
            ONE,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            EIGHT,
            NINE
        };
        public static readonly int[] ALL_WIDTHS =
        {
            ZERO_WIDTH,
            ONE_WIDTH,
            TWO_WIDTH,
            THREE_WIDTH,
            FOUR_WIDTH,
            FIVE_WIDTH,
            SIX_WIDTH,
            SEVEN_WIDTH,
            EIGHT_WIDTH,
            NINE_WIDTH
        };
        public const int HEIGHT = 5;
        public const int MAX_WIDTH = 8;

        public static void WriteSigleDigit(int digit, bool clearSpace, int? startX = null, int? startY = null)
        {
            if (digit < 0 || digit > 9)
                throw new Exception($"'{digit}' Not a single digit");
            startX ??= Console.CursorLeft;
            startY ??= Console.CursorTop;
            string[] numStr = ALL[digit];
            Program.ClearConsoleSpace(HEIGHT);
            for (int i = 0; i < HEIGHT; i++)
            {
                Console.SetCursorPosition(startX.Value, startY.Value + i);
                Console.Write(numStr[i].PadRight(MAX_WIDTH));
            }

        }

        public static void Write(int number, bool clearSpace, int? startX = null, int? startY = null)
        {
            startX ??= Console.CursorLeft;
            startY ??= Console.CursorTop;
            int movingStartX = startX.Value;
            foreach (int digit in Array.ConvertAll(number.ToString().ToCharArray(), c => c - 48))
            {
                WriteSigleDigit(digit, clearSpace, movingStartX, startY);
                movingStartX += ALL_WIDTHS[digit];
            }
        }
    }
}
