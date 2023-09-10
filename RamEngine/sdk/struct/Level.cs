using System.Collections.Generic;

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