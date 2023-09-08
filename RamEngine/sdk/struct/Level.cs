using System.Collections.Generic;

public class Level
{
    public List<SolidObject> children = new List<SolidObject>();

    public void Draw(RenderContext ctx)
    {
        foreach (SolidObject child in children)
        {
            child.Draw(ctx);
        }
    }

    public void Add(SolidObject obj) => children.Add(obj);
    public void Remove(SolidObject obj) => children.Remove(obj);
    public void RemoveBulk(List<SolidObject> list) => children.RemoveAll(x => list.Contains(x));
}