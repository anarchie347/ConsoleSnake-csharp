using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    internal static class CornerUtils
    {
        public static Corner Move(this Corner corner, Direction direction)
        {
            switch (direction){
                case Direction.Up:
                    if (corner == Corner.BottomRight)
                        return Corner.TopRight;
                    else if (corner == Corner.BottomLeft)
                        return Corner.BottomLeft;
                    else
                        return corner;
                case Direction.Right:
                    if (corner == Corner.BottomLeft)
                        return Corner.BottomRight;
                    else if (corner == Corner.TopLeft)
                        return Corner.TopRight;
                    else
                        return corner;
                    
                case Direction.Down:
                    if (corner == Corner.TopRight)
                        return Corner.BottomRight;
                    else if (corner == Corner.TopLeft)
                        return Corner.BottomLeft;
                    else
                        return corner;
                case Direction.Left:
                    if (corner == Corner.TopRight)
                        return Corner.TopLeft;
                    else if (corner == Corner.BottomRight)
                        return Corner.BottomLeft;
                    else
                        return corner;
            }
            return Corner.TopRight;
        }
        public static bool IsOnSide(this Corner corner, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return corner == Corner.TopLeft || corner == Corner.TopRight;
                case Direction.Right:
                    return corner == Corner.TopRight || corner == Corner.BottomRight;
                case Direction.Down:
                    return corner == Corner.BottomLeft || corner == Corner.BottomRight;
                case Direction.Left:
                    return corner == Corner.TopLeft || corner == Corner.BottomLeft;
            }
            return false;
        }
    }
}
