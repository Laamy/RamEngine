using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class SolidObject
{
    // basic stuff
    public Point Position;
    public Size Size;
    public Color? Color;
    public string texturePath = "";
    public List<string> Tags;

    // physics stuff
    public bool Anchored = true;
    public Point Velocity = new Point(0, 0);
    public int Gravity = 1;

    // particle stuff
    public float Speed = -1;
    public float Duration = -1;

    /// <summary>
    /// Set the speed of the particle (if its a particle)
    /// </summary>
    public void SetSpeed(float speed)
        => Speed = speed;

    /// <summary>
    /// Set the duration of the particle in ticks (30 a second for smooth animation)
    /// (if its a particle)
    /// </summary>
    public void SetDuration(float duration) 
        => Duration = duration;

    /// <summary>
    /// The center of the object
    /// </summary>
    public Point Center
    {
        get => new Point(Position.X + (Size.Width / 2), Position.Y + (Size.Height / 2));
    }

    /// <summary>
    /// Quick constructor for a solid object
    /// </summary>
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

    /// <summary>
    /// Quick constructor for a solid object using a texture instead of solid colours
    /// </summary>
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

    /// <summary>
    /// Calculate the distance to a point
    /// </summary>
    public int DistanceTo(Point point)
        => (int)Math.Sqrt(Math.Pow(point.X - Center.X, 2) + Math.Pow(point.Y - Center.Y, 2));

    /// <summary>
    /// Calculate the distance to another object
    /// </summary>
    public int DistanceTo(SolidObject obj)
        => DistanceTo(obj.Center);

    /// <summary>
    /// Calculate the distance to a player
    /// </summary>
    public int DistanceTo(Player player)
        => DistanceTo(player.Center);

    public bool IsCollidingWith(SolidObject solidObject)
    {
        return Position.X < solidObject.Position.X + solidObject.Size.Width &&
               Position.X + Size.Width > solidObject.Position.X &&
               Position.Y < solidObject.Position.Y + solidObject.Size.Height &&
               Position.Y + Size.Height > solidObject.Position.Y;
    }

    public bool ResolveCollisionWith(SolidObject solidObject)
    {
        // resolve the collision with the solid object if the player is colliding
        if (IsCollidingWith(solidObject))
        {
            int overlapX = Math.Min(Position.X + Size.Width, solidObject.Position.X + solidObject.Size.Width) - Math.Max(Position.X, solidObject.Position.X);
            int overlapY = Math.Min(Position.Y + Size.Height, solidObject.Position.Y + solidObject.Size.Height) - Math.Max(Position.Y, solidObject.Position.Y);

            if (overlapX < overlapY)
            {
                // we are colliding with the left/right of the solid object
                if (Position.X < solidObject.Position.X)
                    Position.X -= overlapX;
                else
                    Position.X += overlapX;

                Velocity.X = 0;
            }
            else
            {
                // we are colliding with the top/bottom of the solid object
                if (Position.Y < solidObject.Position.Y)
                {
                    Position.Y -= overlapY;
                }
                else
                {
                    Position.Y += overlapY;
                }

                Velocity.Y = 0;
            }
            return true;
        }

        return false;
    }

    public void Update(GameEngine engine)
    {
        // add gravity to the velocity
        Velocity.Y += Gravity;

        // Add movement to the position based on the velocity
        Position.X += Velocity.X;
        Position.Y += Velocity.Y;
    }

    /// <summary>
    /// Render this object using the render context provided
    /// </summary>
    /// <param name="ctx"></param>
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