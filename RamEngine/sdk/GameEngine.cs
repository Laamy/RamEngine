using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

public class GameEngine : GameWindow
{
    public Dictionary<Key, bool> keymap = new Dictionary<Key, bool>();
    private int targetFPS = 144;
    private int countingFPS = 0;
    private Stopwatch fpsStopwatch;

    public float deltaTime = 0;
    public int GameFPS = 0;
    public ClientInstance Instance = new ClientInstance();

    public GameEngine()
    {
        // setup window properties
        Size = new Size(800, 600);
        Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\assets\\icon\\ramengine-icon.ico");

        // setup extra window properties
        fpsStopwatch = Stopwatch.StartNew();

        // window events
        Load += (s,e) => InitGraphics();
    }

    unsafe void InitGraphics()
    {
        // graphic setup here
        GL.ClearColor(Color4.Black);
        GL.Enable(EnableCap.DepthTest);
        GL.Viewport(0, 0, Width, Height);

        // setup keymap
        for (int i = 0; i < 0xFF; i++)
            keymap.Add((Key)i, false);

        KeyUp += (s,e) => KeyPress(e.Key, false, false);
        KeyDown += (s,e) => KeyPress(e.Key, true, e.IsRepeat);
    }

    #region overridable methods

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        deltaTime = (float)e.Time;

        countingFPS++;
        if (fpsStopwatch.ElapsedMilliseconds >= 500)
        {
            fpsStopwatch = Stopwatch.StartNew();
            GameFPS = countingFPS * 2;
            countingFPS = 0;
        }
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        // call the base render frame function (they probably do some important stuff, idk tbh)
        base.OnRenderFrame(e);

        // clear the screen from the second old frame (old frame is stored in the back buffer)
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        OnUpdate(new RenderContext(e.Time));

        // swap the two buffers out
        SwapBuffers();
    }

    protected virtual void OnUpdate(RenderContext ctx)
    {
        // override this method to draw to the screen
    }

    protected new virtual void KeyPress(Key e, bool held, bool repeat)
    {
        // override this method to handle key presses

        // update the keymap
        keymap[e] = held;
    }

    #endregion

    public void Start()
    {
        Run(targetFPS);
    }
}