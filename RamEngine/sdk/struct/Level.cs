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
}