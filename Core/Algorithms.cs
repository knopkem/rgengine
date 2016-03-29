using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace rgEngine.Core
{
    /// <summary>
    /// Defines common algorithms that can be used everywhere
    /// </summary>
    static class Algorithms
    {
        private static readonly Random Rand = new Random();

        /// <summary>
        /// Generates random directions
        /// </summary>
        /// <param name="exclude">Add Orientations that should not be included</param>
        /// <returns>Either a valid orientation or 0,0 to indicate nothing was found</returns>
        public static Vector2 RandomDirection(List<Vector2> exclude)
        {
            var newOrientation = new Vector2(0, 0);
            
            if (exclude == null)
            {
                return ValidRandomDirection();
            }

            if (exclude.Count < 4)
            {
                do
                {
                    newOrientation = ValidRandomDirection();
                } while (!exclude.Contains(newOrientation));

            }

            return newOrientation;
            
        }

        /// <summary>
        /// Computes the radian value of a vector needed for rotation
        /// </summary>
        /// <param name="vec">the vector</param>
        /// <returns>radian value</returns>
        public static float Vector2Radians(Vector2 vec)
        {
            if (vec.X > 0)
            {
                if (vec.Y >= 0)
                {
                    return (float)Math.Atan(vec.Y / vec.X);
                }
                return (float)Math.Atan(vec.Y / vec.X) + 2 * MathHelper.Pi;
            }
            if (vec.X == 0)
            {
                if (vec.Y > 0)
                {
                    return MathHelper.Pi / 2;
                }
                if (vec.Y == 0)
                {
                    return 0;
                }
                return MathHelper.Pi * 1.5f;
            }
            return (float)Math.Atan(vec.Y / vec.X) + MathHelper.Pi;
        }

        /// <summary>
        /// Checks if position is near the target given the tolerance value
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="target"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsNearTarget(Vector2 pos, Vector2 target, float tolerance)
        {
            if (Vector2.Distance(pos, target) < tolerance)
                return true;
            return false;
        }

        /// <summary>
        /// Needed for grid position to world position mapping 
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public static Point World2Grid(Vector2 worldPos, int tileSize)
        {
            var gridPos = new Point((int)worldPos.X/tileSize, (int)worldPos.Y/tileSize);

            return gridPos;
        }

        /// <summary>
        /// Needed for world to grid position mapping (warning: possible loss of detail) 
        /// </summary>
        /// <param name="gridPos"></param>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public static Vector2 Grid2World(Point gridPos, int tileSize)
        {
            var worldPos = new Vector2(gridPos.X * tileSize, gridPos.Y * tileSize);

            return worldPos;
        }

        #region Private methods

        /// <summary>
        /// generates a random position that is not zero or diagonal
        /// </summary>
        /// <returns></returns>
        private static Vector2 ValidRandomDirection()
        {
            int x = 0;
            int y = 0;
            while ( ((x == 0) && (y == 0)) || ( (Math.Abs(x) == 1) && (Math.Abs(y) == 1) ) )
            {
                x = Rand.Next(-1, 2);
                y = Rand.Next(-1, 2);
            }
            return new Vector2(x, y);
        }

        #endregion

    }
}
