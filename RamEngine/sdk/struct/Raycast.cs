using System.Drawing;
using System.Numerics;

// raycast video: https://www.youtube.com/watch?v=TOEi6T2mtHo
public class Raycast
{
    public bool HasCollided = false;
    public float Dist = 0;

    public SolidObject Hit = null;
    public Vector2 Position;
}