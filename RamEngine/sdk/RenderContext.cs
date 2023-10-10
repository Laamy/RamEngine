using System.Collections.Generic;
using System.Numerics;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

public class RenderContext
{
    private double Time;
    private Dictionary<Color4, List<Vector2>> filledPolygons;
    private Dictionary<Color4, List<Vector2>> outlinedPolygons;

    public RenderContext(double time)
    {
        Time = time;
        filledPolygons = new Dictionary<Color4, List<Vector2>>();
        outlinedPolygons = new Dictionary<Color4, List<Vector2>>();
    }

    public void Clear(Color4 color)
    {
        GL.ClearColor(color.R, color.G, color.B, color.A);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void BeginFrame()
    {
        // Set up any necessary OpenGL state here before rendering.
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public void EndFrame()
    {
        // Render filled polygons
        foreach (var kvp in filledPolygons)
        {
            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(kvp.Key);
            foreach (var vertex in kvp.Value)
            {
                GL.Vertex2(vertex.X, vertex.Y);
            }
            GL.End();
        }

        // Render outlined polygons
        foreach (var kvp in outlinedPolygons)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(kvp.Key);
            foreach (var vertex in kvp.Value)
            {
                GL.Vertex2(vertex.X, vertex.Y);
            }
            GL.End();
        }

        // Clear the polygon lists
        filledPolygons.Clear();
        outlinedPolygons.Clear();
    }

    public void DrawSprite(Vector2 position, Vector2 size, string texturePath)
    {
        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, TextureHandler.GetTexture(texturePath));

        GL.Begin(PrimitiveType.Quads);
        GL.TexCoord2(0f, 0f); GL.Vertex2(position.X, position.Y);
        GL.TexCoord2(1f, 0f); GL.Vertex2(position.X + size.X, position.Y);
        GL.TexCoord2(1f, 1f); GL.Vertex2((position + size).X, (position + size).Y);
        GL.TexCoord2(0f, 1f); GL.Vertex2(position.X, position.Y + size.Y);
        GL.End();

        GL.Disable(EnableCap.Texture2D);
    }

    public void DrawText(string text, Vector2 position, Color4 color, float size = 16, string font = "Arial")
    {
        // Implement text rendering using OpenGL fonts or textures.
        // This depends on your choice of text rendering approach (bitmap fonts, texture-based fonts, etc.).
    }

    public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color4 color, bool filled = true)
    {
        if (filled)
        {
            AddFilledPolygon(color, p1, p2, p3);
        }
        else
        {
            AddOutlinedPolygon(color, p1, p2, p3);
        }
    }

    public void DrawRectangle(Vector2 p1, Vector2 p2, Color4 color, bool filled = true)
    {
        if (filled)
        {
            AddFilledPolygon(color, p1, new Vector2(p2.X, p1.Y), p2, new Vector2(p1.X, p2.Y));
        }
        else
        {
            AddOutlinedPolygon(color, p1, new Vector2(p2.X, p1.Y), p2, new Vector2(p1.X, p2.Y));
        }
    }

    private void AddFilledPolygon(Color4 color, params Vector2[] vertices)
    {
        if (!filledPolygons.ContainsKey(color))
        {
            filledPolygons[color] = new List<Vector2>();
        }

        filledPolygons[color].AddRange(vertices);
    }

    private void AddOutlinedPolygon(Color4 color, params Vector2[] vertices)
    {
        if (!outlinedPolygons.ContainsKey(color))
        {
            outlinedPolygons[color] = new List<Vector2>();
        }

        outlinedPolygons[color].AddRange(vertices);
    }
}