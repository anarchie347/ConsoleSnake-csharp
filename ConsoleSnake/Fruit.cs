using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
	internal class Fruit
	{
		public Point Location { get; private set; }

		private Size GridDimensions;

		public Fruit(Size gridDimensions, IEnumerable<Point> dissallowedPoints)
		{
			GridDimensions = gridDimensions;
			NewLocation(dissallowedPoints);
		}
		public Fruit(Size gridDimensions)
		{
			GridDimensions = gridDimensions;
			NewLocation(Enumerable.Empty<Point>());
		}

		public void NewLocation(IEnumerable<Point> dissallowedPoints)
		{
			Random r = new();
			do
			{
				Location = new Point(r.Next(GridDimensions.Width), r.Next(GridDimensions.Height));
			} while (dissallowedPoints.Contains(Location));
		}
		public void OutputFruit(Size squareSize, Point gridStartLocation)
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(gridStartLocation.X + (squareSize.Width * Location.X), gridStartLocation.Y + (squareSize.Height * Location.Y));
			Console.Write("/ `\\");
            Console.SetCursorPosition(gridStartLocation.X + (squareSize.Width * Location.X), gridStartLocation.Y + (squareSize.Height * Location.Y) + 1);
			Console.Write("\\__/");
        }
	}
}
