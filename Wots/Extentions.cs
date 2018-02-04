using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace Wots
{
    public static class Extentions
    {

        public static float GetHorizontalIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;

            // Calculate centers.
            float centerA = rectA.Left + halfWidthA;
            float centerB = rectB.Left + halfWidthB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA - centerB;
            float minDistanceX = halfWidthA + halfWidthB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX)
                return 0f;

            // Calculate and return intersection depths.
            return distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        }


        public static float GetVerticalIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfHeightA = rectA.Height / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            float centerA = rectA.Top + halfHeightA;
            float centerB = rectB.Top + halfHeightB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceY = centerA - centerB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceY) >= minDistanceY)
                return 0f;

            // Calculate and return intersection depths.
            return distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
        }

        static double EPSILON = 1e-12;

        public static float Map(float valueCoord1,
                float startCoord1, float endCoord1,
                float startCoord2, float endCoord2)
        {

            if (Math.Abs(endCoord1 - startCoord1) < EPSILON)
            {
                throw new ArithmeticException("/ 0");
            }

            float offset = startCoord2;
            float ratio = (endCoord2 - startCoord2) / (endCoord1 - startCoord1);
            return ratio * (valueCoord1 - startCoord1) + offset;
        }
    }
}
