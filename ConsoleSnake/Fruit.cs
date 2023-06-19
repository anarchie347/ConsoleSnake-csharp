using System.Drawing;

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

		public void NewLocation(params IEnumerable<Point>[] dissallowedPoints)
		{
			Random r = new();
			//because the dissallowed points are passed by reference, updating the location updates the disalowed points array containg the location of that fruit, hence the new location will always be in the dissallowed points
			Point[][] staticDissallowed = new Point[dissallowedPoints.Length][];
			for (int i = 0; i < dissallowedPoints.Length; i++)
			{
				staticDissallowed[i] = new Point[dissallowedPoints[i].Count()];
				dissallowedPoints[i].ToArray().CopyTo(staticDissallowed[i],0);
			}
			do
			{
				Location = new Point(r.Next(GridDimensions.Width), r.Next(GridDimensions.Height));
			} while (JaggedContains(staticDissallowed, Location));
		}
		public static bool JaggedContains(IEnumerable<Point>[] jaggedArr, Point value)
		{
            for (int i = 0; i < jaggedArr.Length; i++)
			{
				for (int j = 0; j < jaggedArr[i].Count(); j++)
				{
					if (jaggedArr[i].ToArray()[j] == value)
					{
						return true;
					}
				}
			}
			return false;
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
