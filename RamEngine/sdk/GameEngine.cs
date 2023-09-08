using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;

public class GameEngine : Form
{
    public Dictionary<Keys, bool> keymap = new Dictionary<Keys, bool>();
    private Thread gameLoopThread;
    private int targetFPS = 144;
    private int countingFPS = 0;
    private bool isRunning = false;
    private Stopwatch stopwatch;
    private Stopwatch fpsStopwatch;

    public float deltaTime = 0;
    public int GameFPS = 0;
    public ClientInstance Instance = new ClientInstance();

    public GameEngine()
    {
        // setup window properties
        Size = new Size(800, 600);
        DoubleBuffered = true;
        Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\assets\\icon\\ramengine-icon.ico");

        // window events
        FormClosing += (s, e) => Stop();
        Paint += (s, e) => OnUpdate(new RenderContext(e.Graphics));

        // setup keymap
        for (int i = 0; i < 0xFF; i++)
            keymap.Add((Keys)i, false);

        // key events
        KeyUp += (s, e) => KeyPress(e.KeyCode, false, keymap[e.KeyCode] == false);
        KeyDown += (s, e) => KeyPress(e.KeyCode, true, keymap[e.KeyCode] == true);
    }

    #region overridable methods

    protected virtual void OnUpdate(RenderContext ctx)
    {
        // override this method to draw to the screen
    }

    protected new virtual void KeyPress(Keys e, bool held, bool repeat)
    {
        // override this method to handle key presses

        // update the keymap
        keymap[e] = held;
    }

    #endregion

    public void Stop()
    {
        // stop the game loop & close the window
        isRunning = false;
        gameLoopThread.Join();
    }

    public void Start()
    {
        // start the game loop
        isRunning = true;
        gameLoopThread = new Thread(GameLoop);
        gameLoopThread.Start();

        // when the window exists it causes an exception so lets catch then ignore it
        try
        {
            Application.Run(this);
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.ReadKey();
        }
    }

    private void GameLoop()
    {
        // store the target ticks per frame & the previous ticks
        long targetTicksPerFrame = TimeSpan.TicksPerSecond / targetFPS;
        long prevTicks = DateTime.Now.Ticks;

        stopwatch = Stopwatch.StartNew();
        fpsStopwatch = Stopwatch.StartNew();

        // game loop
        while (isRunning)
        {
            // start counting fps
            countingFPS++;

            // calculate the elapsed ticks & store current tick
            long currTicks = DateTime.Now.Ticks;
            long elapsedTicks = currTicks - prevTicks;

            // calculate deltatime
            deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
            stopwatch = Stopwatch.StartNew();

            // calculate frames per second
            if (fpsStopwatch.ElapsedMilliseconds >= 500)
            {
                fpsStopwatch = Stopwatch.StartNew();
                GameFPS = countingFPS * 2;
                countingFPS = 0;
            }

            // if the elapsed ticks is greater than or equal to the target ticks per frame then update the window
            if (elapsedTicks >= targetTicksPerFrame)
            {
                // update the window & store the current ticks in previous ticks
                prevTicks = currTicks;
                Invalidate();
            }

            // else sleep for 1 millisecond to avoid hogging the cpu
            Thread.Sleep(1);
        }
    }
}