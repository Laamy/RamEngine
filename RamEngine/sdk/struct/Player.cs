using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Numerics;
using System.Windows.Forms;

public class Player
{
    // actor stuff
    public Vector2 Position;
    public Vector2 Size;
    public Color4 Color;

    // main physics stuff
    public Vector2 Velocity = new Vector2(0, 0);
    public int Gravity = 1;

    // movement stuff
    public int JumpForce = -12;
    public int Speed = 5;
    public Vector2 Movement = new Vector2(0, 0);

    // fancy knockback stuff
    private Vector2 knockbackVelocity = Vector2.Zero;
    private int knockbackDuration = 0;
    private int knockbackTimer = 0;

    // extra stuff
    public bool IsOnGround = false;

    /// <summary>
    /// Returns the center of the player
    /// </summary>
    public Vector2 Center
    {
        get => new Vector2(Position.X + (Size.X / 2), Position.Y + (Size.Y / 2));
    }

    /// <summary>
    /// Applies knockback to the player
    /// </summary>
    public void ApplyKnockback(Vector2 knockbackForce, int duration)
    {
        knockbackVelocity = knockbackForce;
        knockbackDuration = duration;
        knockbackTimer = 0;
    }

    /// <summary>
    /// Creates a player
    /// </summary>>
    public Player(Vector2 position, Vector2 size, Color4 color)
    {
        Position = position;
        Size = size;
        Color = color;
    }

    /// <summary>
    /// Makes the player jump from the ground
    /// </summary>
    public void JumpFromGround(float power = 1)
    {
        if (IsOnGround)
        {
            Velocity.Y = (int)(JumpForce * power);
            IsOnGround = false;
        }
    }

    public bool IsCollidingWith(SolidObject solidObject)
    {
        return Position.X < solidObject.Position.X + solidObject.Size.X &&
               Position.X + Size.X > solidObject.Position.X &&
               Position.Y < solidObject.Position.Y + solidObject.Size.Y &&
               Position.Y + Size.Y > solidObject.Position.Y;
    }

    public bool ResolveCollisionWith(SolidObject solidObject)
    {
        // resolve the collision with the solid object if the player is colliding
        if (IsCollidingWith(solidObject))
        {
            int overlapX = (int)(Math.Min(Position.X + Size.X, solidObject.Position.X + solidObject.Size.X) - Math.Max(Position.X, solidObject.Position.X));
            int overlapY = (int)(Math.Min(Position.Y + Size.X, solidObject.Position.Y + solidObject.Size.Y) - Math.Max(Position.Y, solidObject.Position.Y));

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
                    IsOnGround = true;
                    Position.Y -= overlapY;
                }
                else
                {
                    IsOnGround = false;
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

        // add movement to the velocity based on the keymap (WASD)
        if (engine.keymap[Key.A] == true)
            Movement.X--;

        if (engine.keymap[Key.D] == true)
            Movement.X++;


        // Apply knockback if it's active
        if (knockbackTimer < knockbackDuration)
        {
            Velocity.X = knockbackVelocity.X;
            Velocity.Y = knockbackVelocity.Y;
            knockbackTimer++;

            // If the knockback duration is reached, reset the knockback
            if (knockbackTimer == knockbackDuration)
            {
                knockbackVelocity = Vector2.Zero;
                knockbackDuration = 0;
            }
        }
        else
        {
            // Apply constant movement based on the keymap (WASD)
            Velocity.X = Movement.X * Speed;
            // Velocity.Y = Movement.Y * Speed; (You can enable this for vertical movement if needed)

            // Reset the movement vector to zero after applying it
            Movement = new Vector2(0, 0);
        }

        // Add movement to the position based on the velocity
        Position.X += Velocity.X;
        Position.Y += Velocity.Y;

        // reset the movement
        Movement = new Vector2(0, 0);
    }

    public void Draw(RenderContext ctx)
    {
        // draw the player sprite at assets\\ram-sprite-32x32.png
        ctx.DrawSprite(Position, Size, "assets\\ram-sprite-32x32.png");

        // draw a filled rectangle at the positon
        //ctx.DrawRectangle(Position, Size, Color, true);
    }
}