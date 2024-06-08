using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class TextureHandler
{
    private static Dictionary<string, Image> textures = new Dictionary<string, Image>();

    public static Image GetTexture(string path)
    {
        if (textures.ContainsKey(path))
            return textures[path];

        textures.Add(path, Image.FromFile(Application.StartupPath + "\\" + path));

        // this can possibly cause recursion even though it shouldn't
        return GetTexture(path);
    }

    public static void ReloadTextures() => textures.Clear();
}