using System;
using System.Collections.Generic;
using System.Drawing;

public class SolidObject
{
    public Point Position;
    public Size Size;
    public Color? Color;
    public string texturePath = "";
    public List<string> Tags;

    public SolidObject(Point position, Size size, Color color, List<string> tags = null)
    {
        Position = position;
        Size = size;
        Color = color;
        texturePath = null;

        if (tags == null)
            Tags = new List<string>();
        else Tags = tags;
    }

    public SolidObject(Point position, Size size, string texture, List<string> tags = null)
    {
        Position = position;
        Size = size;
        Color = null;
        texturePath = texture;

        if (tags == null)
            Tags = new List<string>();
        else Tags = tags;
    }

    public int DistanceTo(SolidObject obj)
    {
        return (int)Math.Sqrt(Math.Pow(obj.Position.X - Position.X, 2) + Math.Pow(obj.Position.Y - Position.Y, 2));
    }

    public int DistanceToPoint(Point point)
    {
        return (int)Math.Sqrt(Math.Pow(point.X - Position.X, 2) + Math.Pow(point.Y - Position.Y, 2));
    }

    public void Draw(RenderContext ctx)
    {
        if (Color == null)
        {
            // texture path, draw the texture
            ctx.DrawSprite(Position, Size, texturePath);
        }
        else
        {
            // no texture path, draw a rectangle
            Color color = Color ?? System.Drawing.Color.Transparent;
            ctx.DrawRectangle(Position, Size, color, true);
        }
    }
}