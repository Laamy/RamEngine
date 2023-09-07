using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;

// an optimized graphics object for our game engine
public class RenderContext
{
    private Graphics _g;
    private Dictionary<Color, GraphicsPath> _outlinedPaths = new Dictionary<Color, GraphicsPath>();
    private Dictionary<Color, GraphicsPath> _filledPaths = new Dictionary<Color, GraphicsPath>();

    public RenderContext(Graphics g)
    {
        // store the graphics object
        _g = g;

        _g.SmoothingMode = SmoothingMode.AntiAlias;
    }

    public void DrawSprite(Point p1, Size s1, string sprite)
    {
        // draw the sprite image using graphics path
        // giving up on this for now

        _g.DrawImage(Image.FromFile(sprite), new Rectangle(p1, s1));

        //GraphicsPath path = new GraphicsPath();
        //path.AddRectangle(new Rectangle(p1, s1));
        //DrawPath(Color.White, path);
    }

    public void Clear(Color color)
    {
        // clear the screen using the color
        _g.Clear(color);
    }

    public SizeF MeasureText(string text, float size, string fontName)
    {
        // create a new font object (TODO: cache this)
        using (Font font = new Font(fontName, size))
        {
            // return the size of the text
            return _g.MeasureString(text, font);
        }
    }

    public void DrawText(string text, Point p1, Color colour, float size = 16, string font = "Arial")
    {
        // draw the text using a graphics path
        GraphicsPath path = new GraphicsPath();
        path.AddString(text, new FontFamily(font), 3, size, p1, StringFormat.GenericDefault);
        DrawPath(colour, path);
    }

    public void DrawTriangle(Point p1, Point p2, Point p3, Color colour, bool filled = true)
    {
        if (!filled)
        {
            // add the 3 lines to the graphics path
            DrawLine(colour, p1, p2);
            DrawLine(colour, p2, p3);
            DrawLine(colour, p3, p1);
        }
        else
        {
            // draw a filled in triangle using FillPath
            GraphicsPath path = new GraphicsPath();
            path.AddLine(p1, p2);
            path.AddLine(p2, p3);
            path.AddLine(p3, p1);
            FillPath(colour, path);
        }
    }

    public void DrawRectangle(Point p1, Size s1, Color colour, bool filled = true)
    {
        if (!filled)
        {
            // draw an outline of a rectangle using DrawLine function
            DrawLine(colour, p1, new Point(p1.X + s1.Width, p1.Y));
            DrawLine(colour, new Point(p1.X + s1.Width, p1.Y), new Point(p1.X + s1.Width, p1.Y + s1.Height));
            DrawLine(colour, new Point(p1.X + s1.Width, p1.Y + s1.Height), new Point(p1.X, p1.Y + s1.Height));
            DrawLine(colour, new Point(p1.X, p1.Y + s1.Height), p1);
        }
        else
        {
            // draw a filled rectangle using FillPath
            GraphicsPath path = new GraphicsPath();
            path.AddLine(p1, new Point(p1.X + s1.Width, p1.Y));
            path.AddLine(new Point(p1.X + s1.Width, p1.Y), new Point(p1.X + s1.Width, p1.Y + s1.Height));
            path.AddLine(new Point(p1.X + s1.Width, p1.Y + s1.Height), new Point(p1.X, p1.Y + s1.Height));
            path.AddLine(new Point(p1.X, p1.Y + s1.Height), p1);
            FillPath(colour, path);
        }
    }

    private void DrawLine(Color colour, Point p1, Point p2)
    {
        // create a new graphics path
        GraphicsPath path = new GraphicsPath();

        // add the line to the path
        path.AddLine(p1, p2);

        // draw the path
        DrawPath(colour, path);
    }

    public void DrawPath(Color pen, GraphicsPath path)
    {
        // draw the path using the pen
        if (!_outlinedPaths.ContainsKey(pen))
        {
            // create a new graphics path cuz we don't have one for this colour yet
            _outlinedPaths[pen] = path;
        }
        else
        {
            // combine the graphics paths
            _outlinedPaths[pen].AddPath(path, false);
        }
    }

    public void FillPath(Color pen, GraphicsPath path)
    {
        // draw the path using the pen
        if (!_filledPaths.ContainsKey(pen))
        {
            // create a new graphics path cuz we don't have one for this colour yet
            _filledPaths[pen] = path;
        }
        else
        {
            // combine the graphics paths
            _filledPaths[pen].AddPath(path, false);
        }
    }

    public void EndFrame(float scale = 1)
    {
        // scale before drawing
        _g.ScaleTransform(scale, scale);

        foreach (KeyValuePair<Color, GraphicsPath> path in _outlinedPaths)
        {
            // draw the path using the pen
            _g.DrawPath(new Pen(path.Key), path.Value);
        }

        foreach (KeyValuePair<Color, GraphicsPath> path in _filledPaths)
        {
            // draw the path using the pen
            _g.FillPath(new SolidBrush(path.Key), path.Value);
        }

        // clear the paths
        _outlinedPaths.Clear();
    }
}