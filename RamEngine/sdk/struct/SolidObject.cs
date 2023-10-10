using OpenTK.Graphics;
using RamEngine.sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Management.Instrumentation;
using System.Numerics;

public class SolidObject
{
    public Vector2 Position { get; private set; }
    public Vector2 Size { get; private set; }
    public Color4 Color { get; private set; }
    public string TexturePath { get; private set; }
    public bool Anchored { get; set; } = true;
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    /// <summary>
    /// The center of the object
    /// </summary>
    public Vector2 Center
    {
        get => new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));
    }

    /// <summary>
    /// Quick constructor for a solid object
    /// </summary>
    public SolidObject(Vector2 position, Vector2 size, Color4 color, string texturePath = "")
    {
        Position = position;
        Size = size;
        Color = color;
        TexturePath = texturePath;
    }

    /// <summary>
    /// Calculate the distance to a point
    /// </summary>
    public int DistanceTo(Vector2 point)
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

    public void Move(Vector2 delta)
    {
        Position += delta;
    }

    public void Update(float deltatime)
    {
        // Add gravity to the velocity (if needed)
        // Velocity.Y += Gravity;

        // Update the position based on the velocity
        Position += Velocity * deltatime;
    }

    public bool IsCollidingWith(SolidObject solidObject)
    {
        return Position.X < solidObject.Position.X + solidObject.Size.X &&
               Position.X + Size.X > solidObject.Position.X &&
               Position.Y < solidObject.Position.Y + solidObject.Size.Y &&
               Position.Y + Size.Y > solidObject.Position.Y;
    }

    public bool IsCollidingWith(Vector2 point)
        => IsCollidingWith(new SolidObject(point, new Vector2(1, 1), System.Drawing.Color.Black));

    public bool ResolveCollisionWith(SolidObject solidObject)
    {
        // resolve the collision with the solid object if the player is colliding
        if (IsCollidingWith(solidObject))
        {
            int overlapX = (int)(Math.Min(Position.X + Size.X, solidObject.Position.X + solidObject.Size.X) - Math.Max(Position.X, solidObject.Position.X));
            int overlapY = (int)(Math.Min(Position.Y + Size.Y, solidObject.Position.Y + solidObject.Size.Y) - Math.Max(Position.Y, solidObject.Position.Y));

            if (overlapX < overlapY)
            {
                // we are colliding with the left/right of the solid object
                if (Position.X < solidObject.Position.X)
                    Position -= new Vector2() { X = overlapX };
                else
                    Position += new Vector2() { X = overlapX };
            }
            else
            {
                // we are colliding with the top/bottom of the solid object
                if (Position.Y < solidObject.Position.Y)
                {
                    Position -= new Vector2() { Y = overlapY };
                }
                else
                {
                    Position += new Vector2() { Y = overlapY };
                }

                Velocity.SetY(0);
            }
            return true;
        }

        return false;
    }

    public void Update(GameEngine engine)
    {
        // add gravity to the velocity
        Velocity.SetY(Velocity.Y + 1f);

        // Add movement to the position based on the velocity
        Position.SetX(Position.X + Velocity.X);
        Position.SetY(Position.Y + Velocity.Y);
    }

    /// <summary>
    /// Render this object using the render context provided
    /// </summary>
    /// <param name="ctx"></param>

    public void Draw(RenderContext ctx)
    {
        if (!string.IsNullOrEmpty(TexturePath))
        {
            // Draw the texture
            ctx.DrawSprite(Position, Size, TexturePath);
        }
        else
        {
            // Draw a colored rectangle
            ctx.DrawRectangle(Position, Size, Color, true);
        }
    }
}