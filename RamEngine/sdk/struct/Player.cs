using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

public class Player
{
    // actor stuff
    public Point Position;
    public Size Size;
    public Color Color;

    // main physics stuff
    public Point Velocity = new Point(0, 0);
    public int Gravity = 1;

    // movement stuff
    public int JumpForce = -12;
    public int Speed = 5;
    public Point Movement = new Point(0, 0);

    // fancy knockback stuff
    private Point knockbackVelocity = Point.Empty;
    private int knockbackDuration = 0;
    private int knockbackTimer = 0;

    // extra stuff
    public bool IsOnGround = false;

    public void ApplyKnockback(Point knockbackForce, int duration)
    {
        knockbackVelocity = knockbackForce;
        knockbackDuration = duration;
        knockbackTimer = 0;
    }

    public Player(Point position, Size size, Color color)
    {
        Position = position;
        Size = size;
        Color = color;
    }

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
        if (engine.keymap[Keys.A] == true)
            Movement.X--;

        if (engine.keymap[Keys.D] == true)
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
                knockbackVelocity = Point.Empty;
                knockbackDuration = 0;
            }
        }
        else
        {
            // Apply constant movement based on the keymap (WASD)
            Velocity.X = Movement.X * Speed;
            // Velocity.Y = Movement.Y * Speed; (You can enable this for vertical movement if needed)

            // Reset the movement vector to zero after applying it
            Movement = new Point(0, 0);
        }

        // Add movement to the position based on the velocity
        Position.X += Velocity.X;
        Position.Y += Velocity.Y;

        // reset the movement
        Movement = new Point(0, 0);
    }

    public void Draw(RenderContext ctx)
    {
        // draw a filled rectangle at the positon
        ctx.DrawRectangle(Position, Size, Color, true);
    }
}