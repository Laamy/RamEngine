using RamEngine.sdk;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

public class Level
{
    public List<SolidObject> children = new List<SolidObject>();

    /// <summary>
    /// Draws the levels children
    /// </summary>
    public void Draw(RenderContext ctx)
    {
        foreach (SolidObject child in children)
        {
            child.Draw(ctx);
        }
    }

    /// <summary>
    /// Casts a ray from a point in a direction (with optional distance)
    /// </summary>
    public Raycast CastRay(Vector2 start, Vector2 end)
    {
        // Create a new ray
        Raycast ray = new Raycast();

        // Calculate the direction vector
        Vector2 direction = new Vector2(end.X - start.X, end.Y - start.Y);
        int absX = (int)Math.Abs(direction.X);
        int absY = (int)Math.Abs(direction.Y);

        // Determine the step direction for X and Y
        int stepX = (direction.X > 0) ? 1 : -1;
        int stepY = (direction.Y > 0) ? 1 : -1;

        // Calculate initial values for the decision parameter and pixel coordinates
        int x = (int)start.X;
        int y = (int)start.Y;
        int decision;

        // Calculate the distance
        int distance = start.Distance(end);

        // list all the solid objects in level children within "distance" int
        List<SolidObject> objects = new List<SolidObject>();
        foreach (SolidObject child in children)
        {
            if (child.DistanceTo(start) < distance)
            {
                objects.Add(child);
            }
        }

        // Loop through all the pixels from start to end
        while (x != end.X || y != end.Y)
        {
            if (ray.Dist >= distance)
                return ray;

            // Check if the current pixel is solid
            foreach (SolidObject child in objects)
            {
                if (child.IsCollidingWith(new Vector2(x, y)))
                {
                    // Set the ray result
                    ray.HasCollided = true;
                    ray.Hit = child;
                    ray.Position = new Vector2(x, y);

                    // Return the ray result
                    return ray;
                }
            }

            // Move in the direction of the line
            if (absX >= absY)
            {
                // Increment x and update decision parameter
                x += stepX;
                decision = absY - absX;
            }
            else
            {
                // Increment y and update decision parameter
                y += stepY;
                decision = absX - absY;
            }

            // Increment the distance
            ray.Dist++;

            // Check if we should move diagonally
            if (decision > 0)
            {
                x += stepX;
                y += stepY;
                ray.Dist++;
            }
        }

        // Return the ray result
        return ray;
    }

    /// <summary>
    /// Update the physics of the levels children
    /// </summary>
    public void Update(GameEngine game)
    {
        foreach (SolidObject child in children)
        {
            if (!child.Anchored)
            {
                child.Update(game);
            }
        }
    }

    /// <summary>
    /// Adds a child to the level
    /// </summary>
    public void Add(SolidObject obj) => children.Add(obj);

    /// <summary>
    /// Removes a child from the level
    /// </summary>
    public void Remove(SolidObject obj) => children.Remove(obj);

    /// <summary>
    /// Removes a list of children from the level (use this instead of looping over and fucking up the list)
    /// </summary>
    public void RemoveBulk(List<SolidObject> list) => children.RemoveAll(x => list.Contains(x));
}