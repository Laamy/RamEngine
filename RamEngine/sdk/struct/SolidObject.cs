using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class SolidObject
{
    public Point Position;
    public Size Size;
    public Color Color;
    public List<string> Tags;

    public SolidObject(Point position, Size size, Color color, List<string> tags = null)
    {
        Position = position;
        Size = size;
        Color = color;

        if (tags == null)
            Tags = new List<string>();
        else Tags = tags;
    }

    public void Draw(RenderContext ctx)
    {
        ctx.DrawRectangle(Position, Size, Color, true);
    }
}