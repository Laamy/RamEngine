using System;
using System.Numerics;

namespace RamEngine.sdk
{
    // C# extension for point that lets you measure distance
    public static class Extensions
    {
        public static int Distance(this Vector2 p1, Vector2 p2)
            => (int)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

        public static void SetY(this Vector2 p1, int y) => p1.Y = y;
        public static void SetX(this Vector2 p1, int x) => p1.X = x;

        public static void SetY(this Vector2 p1, float y) => p1.Y = y;
        public static void SetX(this Vector2 p1, float x) => p1.X = x;
    }
}
