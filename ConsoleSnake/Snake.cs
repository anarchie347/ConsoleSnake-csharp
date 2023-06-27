using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleSnake
{
    internal class Snake
    {
        //public delegate void SnakeMoveEventHandler(object? sender, SnakeMovedEventArgs args);
        public event EventHandler SnakeMove;

        public const ConsoleColor SNAKE_BODY_COLOUR = ConsoleColor.Blue;
        public const ConsoleColor SNAKE_HEAD_COLOUR = ConsoleColor.DarkBlue;
        public const string FACE_TEXT = "ಠ_ಠ";

        //[0] in list is tail, [count - 1] is head
        public ReadOnlyCollection<Point> Coords { get { return Array.AsReadOnly(coords.ToArray()); } }
        public ReadOnlyCollection<Direction> MoveList { get { return Array.AsReadOnly(moveList.ToArray()); } }

        private List<Point> coords;
        private List<Direction> moveList;

        public Point BehindSnakeCoords { get; private set; }
        public Point BehindHeadCoords { get; private set; }
        public int MoveDelay { get; private set; }
        public Corner FacePosition { get; private set; }
        public Direction Direction { get; private set; }
        public bool IsFrozen { get { return !(timer?.Enabled ?? false); } }
        public bool Cheese { get; private set; }


        private System.Timers.Timer timer;
        private bool GrowOnNextTurn;
        private bool CheeseEndRemoveAlternator;


        public Snake(List<Point> initalSnake, int moveDelay, bool cheese)
        {
            coords = initalSnake;
            moveList = new List<Direction>();
            Direction = Direction.Right;
            FacePosition = Corner.TopRight;
            Cheese = cheese;
            

            for (int i = 0; i < initalSnake.Count - 1; i++)
                moveList.Add(Direction.Right);

            timer = new System.Timers.Timer()
            {
                Interval = moveDelay,
                AutoReset = true,
                Enabled = false
            };
            MoveDelay = moveDelay;
            timer.Elapsed += (object sender, ElapsedEventArgs e) => SnakeTimerTick();
            timer.Start();
        }

        public bool ChangeDirection(Direction newDirection, bool muted)
        {
            if ((int)MoveList.Last() == ((int)newDirection + 2) % 4 || MoveList.Last() == newDirection)
            {
                return false;
            }
            Direction = newDirection;
            if (muted)
                return true;
            if (OperatingSystem.IsWindows())
            {
                switch (newDirection)
                {
                    case Direction.Right:
                        Console.Beep(700, 150);
                        break;
                    case Direction.Down:
                        Console.Beep(500, 150);
                        break;
                    case Direction.Left:
                        Console.Beep(600, 150);
                        break;
                    case Direction.Up:
                        Console.Beep(800, 150);
                        break;

                }
            } else
            {
                Console.Beep();
            }
            
            return true;
        }

        public void Grow()
        {
            GrowOnNextTurn = true;
        }
        public void Freeze()
        {
            timer.Stop();
        }
        public void Unfreeze()
        {
            timer.Start();
        }

        public void SetHeadPosition(Point newLocation)
        {
            coords[coords.Count - 1] = newLocation;
        }
        public void MoveOnce()
        {
            SnakeTimerTick();
        }

        private void SnakeTimerTick()
        {
            if (!GrowOnNextTurn)
            { } //remove


            if (!(GrowOnNextTurn || (CheeseEndRemoveAlternator && Cheese)))
            {
                BehindSnakeCoords = coords[0];
                coords.RemoveAt(0);
            }
            if (!GrowOnNextTurn)
                CheeseEndRemoveAlternator = !CheeseEndRemoveAlternator;
            GrowOnNextTurn = false;

            Point currentHeadPos = coords.Last();
            switch (Direction)
            {
                case Direction.Up:
                    coords.Add(new Point(currentHeadPos.X, currentHeadPos.Y - 1));
                    break;
                case Direction.Right:
                    coords.Add(new Point(currentHeadPos.X + 1, currentHeadPos.Y));
                    break;
                case Direction.Down:
                    coords.Add(new Point(currentHeadPos.X, currentHeadPos.Y + 1));
                    break;
                case Direction.Left:
                    coords.Add(new Point(currentHeadPos.X - 1, currentHeadPos.Y));
                    break;
            }
            FacePosition = FacePosition.Move(Direction);
            BehindHeadCoords = currentHeadPos;
            //Point behindheadCoords = coords[coords.Count - 2];
            if (Cheese && (currentHeadPos.X % 2 == currentHeadPos.Y % 2))
                coords.RemoveAt(coords.Count - 2);
            
            moveList.Add(Direction);
            SnakeMove?.Invoke(this, EventArgs.Empty);
        }
    }

    //internal class SnakeMovedEventArgs : EventArgs
    //{
    //    public List<Point> Coords { get; set; }
    //    public Point BehindSnake { get; set; }
    //    public SnakeMovedEventArgs(List<Point> coords, Point behindSnake)
    //    {
    //        Coords = coords;
    //        BehindSnake = behindSnake;
    //    }
    //}
}
