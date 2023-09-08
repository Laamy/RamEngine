using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class TextureHandler
{
    public static Dictionary<string, Image> textures = new Dictionary<string, Image>();

    public static Image GetTexture(string path)
    {
        if (textures.ContainsKey(path))
            return textures[path];

        Image texture = Image.FromFile(Application.StartupPath + "\\" + path);
        textures.Add(path, texture);

        return texture;
    }
}