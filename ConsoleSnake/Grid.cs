using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace ConsoleSnake
{

	internal class Grid
	{
		public event EventHandler FruitEaten;
		public event EventHandler SnakeDied;
		//public const ConsoleColor BACKGROUND_COLOUR_1 = ConsoleColor.Green;
		//public const ConsoleColor BACKGROUND_COLOUR_2 = ConsoleColor.DarkGreen;
		private ColourOptions Colours;
		//do not change these because graphics use these values
		public const int SQUARE_HEIGHT = 2;
		public const int SQUARE_WIDTH = 4;
		private readonly  string SQUARE_LINE_TEXT = new string(' ', SQUARE_WIDTH); 
		public Size Dimensions { get;}
		public Point StartPoint { get;}
		private Snake? Snake;
		private Random rand;

		public bool IsSnakeFrozen { get { return Snake?.IsFrozen ?? false; } }
		public ReadOnlyCollection<Point> SnakeCoords { get { return Snake.Coords; } }
		

		private readonly Fruit[] Fruit;

		public Grid(Size dimensions, Point startPoint, int fruitCount, ColourOptions colours)
		{
			Dimensions = dimensions;
			StartPoint = startPoint;
			Colours = colours;
			Fruit = new Fruit[fruitCount];
			rand = new Random();
			for (int i = 0; i < fruitCount; i++)
			{
				Fruit[i] = new(dimensions, colours.Fruit);
			}
		}

		public void OutputGrid()
		{
			ConsoleColor[] rowColours;
			for (int i = 0; i < Dimensions.Height; i++)
			{
				rowColours = new ConsoleColor[Dimensions.Width];
				for (int j = 0; j < SQUARE_HEIGHT; j++)
				{
					Console.SetCursorPosition(StartPoint.X, StartPoint.Y + i * SQUARE_HEIGHT + j);
					for (int k = 0; k < Dimensions.Width; k++)
					{
						if (j == 0) //makes sure that the cell colour is only decided once, and this value is used for all rows drawing that cell
						{
							rowColours[k] = GetSquareBackgroundColour(i, k);
						}
						Console.BackgroundColor = rowColours[k];
						Console.Write(SQUARE_LINE_TEXT);
					}
					
				}
			}
			foreach (Fruit f in Fruit)
				f.OutputFruit(new Size(SQUARE_WIDTH, SQUARE_HEIGHT), StartPoint);
			if (Snake != null) RenderEntireSnake(Snake.Coords, Snake.FacePosition);
		}

		public void AddSnake(Snake snake, bool pacifist, bool cheese)
		{

			this.Snake = snake;
			RenderEntireSnake(snake.Coords, snake.FacePosition);
			snake.Freeze();
			foreach (Fruit f in Fruit)
			{
				f.NewLocation(snake.Coords, new Point[] { snake.BehindSnakeCoords }, Fruit.Select(f => f.Location), snake.HiddenCheeseCoords);
				f.OutputFruit(new Size(SQUARE_WIDTH, SQUARE_HEIGHT), StartPoint);
			}
			if (pacifist)
			{
				Snake.SnakeMove += (object? sender, EventArgs e) =>
				{
					if (sender is Snake snake)
					{
						WrapSnake(snake);
						CheckIfSnakeHasEatenFruit(snake);
						UpdateSnake(snake);
					}
				};
			}
			else
			{
				Snake.SnakeMove += (object? sender, EventArgs e) =>
				{
					if (sender is Snake snake)
					{
						CheckIfSnakeHasEatenFruit(snake);
						if (CheckIfSnakeHasDied(snake.Coords.Last()))
							SnakeDied?.Invoke(this, EventArgs.Empty);
						else
							UpdateSnake(snake);
					}
				};
			}
		}
		
		public void StartSnake()
		{
			Snake?.Unfreeze();
		}
		public void StopSnake(bool ReOutputGrid)
		{
			Snake?.Freeze();
			if (ReOutputGrid)
                OutputGrid();
				
		}

		public void ChangeSnakeDirection(Direction newDirection, bool muted)
		{
			if (!IsSnakeFrozen)
				Snake.ChangeDirection(newDirection, muted);
		}

		public void MoveSnakeOnce()
		{
			Snake.MoveOnce();
        }

		private void CheckIfSnakeHasEatenFruit(Snake snake)
		{
			foreach (Fruit f in Fruit)
			{

				if (snake.Coords.Last() == f.Location)
				{
					snake.Grow();
					f.NewLocation(snake.Coords, new Point[] { snake.BehindSnakeCoords }, Fruit.Select(f => f.Location), snake.HiddenCheeseCoords);
					f.OutputFruit(new Size(SQUARE_WIDTH, SQUARE_HEIGHT), StartPoint);
					FruitEaten?.Invoke(this, EventArgs.Empty);
				}
			}

		}
		private bool CheckIfSnakeHasDied(Point head)
		{
			return head.X < 0 || head.X >= Dimensions.Width || head.Y < 0 || head.Y >= Dimensions.Height || CheckSnakeSelfCollisions(head);
		}

		private bool CheckSnakeSelfCollisions(Point head)
		{
			return Snake.Coords.IndexOf(head) < Snake.Coords.Count - 1;
		}

		private void WrapSnake(Snake snake)
		{
			Point head = snake.Coords.Last();
			if (head.X < 0)
				snake.SetHeadPosition(new Point(Dimensions.Width - 1, head.Y));
			else if (head.X >= Dimensions.Width)
				snake.SetHeadPosition(new Point(0, head.Y));

			else if (head.Y < 0)
				snake.SetHeadPosition(new Point(head.X, Dimensions.Height - 1));
			else if (head.Y >= Dimensions.Height)
				snake.SetHeadPosition(new Point(head.X, 0));

		}

		private void UpdateSnake(Snake snake)
		{
			Point[] snakeCoords = snake.Coords.ToArray();
			Corner facePosition = snake.FacePosition;
			//[0] in list is tail, [count - 1] is head
			Point editPoint = snake.BehindSnakeCoords;
			//behind tail
			if (!snakeCoords.Contains(editPoint))
			{
				Console.BackgroundColor = GetSquareBackgroundColour(editPoint);
				for (int i = 0; i < SQUARE_HEIGHT; i++)
				{
					Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT + i);
					Console.WriteLine(SQUARE_LINE_TEXT);
				}
			}
			//head
			Console.BackgroundColor = Utils.GetConsoleColor(Snake.SnakeHeadColour, rand);
			editPoint = snakeCoords.Last();
			Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT);
			if (SQUARE_HEIGHT > 1)
			{

				if (facePosition.IsOnSide(Direction.Up))
					if (facePosition.IsOnSide(Direction.Left))
						Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
					else
						Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
				else
					Console.Write(SQUARE_LINE_TEXT);
				for (int i = 1; i < SQUARE_HEIGHT - 1; i++)
				{
					Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT + i);
					Console.Write(SQUARE_LINE_TEXT);
				}
				Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + (editPoint.Y + 1) * SQUARE_HEIGHT - 1);
				if (facePosition.IsOnSide(Direction.Down))
					if (facePosition.IsOnSide(Direction.Left))
						Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
					else
						Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
				else
					Console.Write(SQUARE_LINE_TEXT);
			}
			else
			{
				if (facePosition.IsOnSide(Direction.Left))
					Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
				else
					Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
			}
		    if (snake.Cheese)
			{
				editPoint = snake.BehindHeadCoords;
				Console.BackgroundColor = GetSquareBackgroundColour(editPoint);
                for (int i = 0; i < SQUARE_HEIGHT; i++)
                {
                    Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT + i);
                    Console.WriteLine(SQUARE_LINE_TEXT);
                }
            }

			//point behind head
            editPoint = snakeCoords[snakeCoords.Count() - 2];
            Console.BackgroundColor = Utils.GetConsoleColor(Snake.SnakeBodyColour, rand);
			for (int i = 0; i < SQUARE_HEIGHT; i++)
			{
				Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT + i);
				Console.WriteLine(SQUARE_LINE_TEXT);
			}
		}

		

		private void RenderEntireSnake(IEnumerable<Point> snakeCoords, Corner facePosition)
		{
			//render from back of snake to frojnt, so that if it overlaps, the frontmost part is shown first
			Point editPoint;
            //rest of snake
            for (int i = snakeCoords.Count() -1; i > -1; i--)
            {
                Console.BackgroundColor = Utils.GetConsoleColor(Snake.SnakeBodyColour, rand);
                editPoint = snakeCoords.ElementAt(i);
                for (int j = 0; j < SQUARE_HEIGHT; j++)
                {
                    Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT + j);
                    Console.WriteLine(SQUARE_LINE_TEXT);
                }
            }

			//head
            Console.BackgroundColor = Utils.GetConsoleColor(Snake.SnakeHeadColour, rand);
			editPoint = snakeCoords.Last();
			Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT);
			if (SQUARE_HEIGHT > 1)
			{

				if (facePosition.IsOnSide(Direction.Up))
					if (facePosition.IsOnSide(Direction.Left))
						Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
					else
						Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
				else
					Console.Write(SQUARE_LINE_TEXT);
				for (int i = 1; i < SQUARE_HEIGHT - 1; i++)
				{
					Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + editPoint.Y * SQUARE_HEIGHT + i);
					Console.Write(SQUARE_LINE_TEXT);
				}
				Console.SetCursorPosition(StartPoint.X + editPoint.X * SQUARE_WIDTH, StartPoint.Y + (editPoint.Y + 1) * SQUARE_HEIGHT - 1);
				if (facePosition.IsOnSide(Direction.Down))
					if (facePosition.IsOnSide(Direction.Left))
						Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
					else
						Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
				else
					Console.Write(SQUARE_LINE_TEXT);
			}
			else
			{
				if (facePosition.IsOnSide(Direction.Left))
					Console.Write(Snake.FACE_TEXT.PadRight(SQUARE_WIDTH));
				else
					Console.Write(Snake.FACE_TEXT.PadLeft(SQUARE_WIDTH));
			}
			
		}

		private ConsoleColor GetSquareBackgroundColour(Point p)
		{
			return GetSquareBackgroundColour(p.X, p.Y);
        }
		private ConsoleColor GetSquareBackgroundColour(int x, int y)
		{
            return (x % 2 == y % 2) ? Utils.GetConsoleColor(Colours.Background1, rand) : Utils.GetConsoleColor(Colours.Background2, rand);
        }
	}
}
