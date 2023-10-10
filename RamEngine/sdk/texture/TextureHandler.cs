using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

public class TextureHandler
{
    private static Dictionary<string, int> textures = new Dictionary<string, int>();

    public static int GetTexture(string path)
    {
        if (textures.ContainsKey(path))
            return textures[path];

        int textureId = LoadTexture(path);
        textures.Add(path, textureId);
        return textureId;
    }

    private static int LoadTexture(string path)
    {
        int textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        using (Bitmap bmp = new Bitmap(path))
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
        }

        // Set texture parameters (you can customize these as needed)
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return textureId;
    }

    public static void ReloadTextures() => textures.Clear();
}