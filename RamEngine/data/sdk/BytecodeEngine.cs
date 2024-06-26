using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class BytecodeEngine : GameEngine
{
    public ScriptContext context = new ScriptContext();

    public BytecodeEngine()
    {
        // setup extra window properties
        Text = "BytecodeEngine";

        // disable resizing
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        
        // setup menustrip
        MenuStrip menu = new MenuStrip();

        {
            // create file & exit button in file
            ToolStripMenuItem file = new ToolStripMenuItem("File");

            {
                file.DropDownItems.Add("Exit", null, (s, e) => Close());
                menu.Items.Add(file);
            }
        }

        {
            // create debug & otehr stuff in debug
            ToolStripMenuItem debug = new ToolStripMenuItem("Debug");

            {
                debug.DropDownItems.Add("Reload Packs", null, (s, e) => TextureHandler.ReloadTextures());
                menu.Items.Add(debug);
            }
        }

        Controls.Add(menu);

        Console.WriteLine("Building states..");
        // setup the state
        context.InitState();

        //loop over all 4 window border walls and add solid objects to stop players from escaping them

        // floor
        //Instance.GetLevel().children.Add(new SolidObject( new Point(0, Height - (menu.Size.Height * 2)), new Size(Width, 10), Color.White ));

        // world stuff
        int blockSize = 15;

        Console.WriteLine("Building level..");
        BlockType[][] world = TerrainGen.GenerateWorld(WorldType.Flat, Width / blockSize, Height / blockSize, 7, 0.08f);

        Console.WriteLine("Building terrain..");
        // foreach layer
        TerrainGen.GrowWorld(world, blockSize, Instance);

        // jumppad
        Instance.GetLevel().children.Add(new SolidObject(
            new Point(50, Height - (menu.Size.Height * 2) - 10),
            new Size(30, 11),
            Color.Red,
            new List<string> { "JumpPad" }
        ));

        // start the game winodw & engine
        Console.WriteLine("Creating graphics..");
        Start();
    }

    protected override void KeyPress(Keys e, bool held, bool repeat)
    {
        if (e == Keys.Space)
            Instance.GetLocalPlayer().JumpFromGround();

        if (e == Keys.O)
        {
            List<SolidObject> objectsToRemove = new List<SolidObject>();

            foreach (SolidObject obj in Instance.GetLevel().children)
            {
                if (obj.Tags.Contains("Breakable") && obj.DistanceTo(Instance.GetLocalPlayer().Center) < 32)
                {
                    objectsToRemove.Add(obj);
                }
            }

            Instance.GetLevel().RemoveBulk(objectsToRemove);
        }

        if (e == Keys.P)
        {
            var playerPosition = Instance.GetLocalPlayer().Center;
            int playerX = playerPosition.X;
            int playerY = playerPosition.Y;

            int gridSize = 15;

            for (int dx = -32; dx <= 32; dx += gridSize)
            {
                for (int dy = -32; dy <= 32; dy += gridSize)
                {
                    int x = (playerX + dx) / gridSize * gridSize;
                    int y = (playerY + dy) / gridSize * gridSize;
                    y += 10; // for some reason its off by 10..?

                    if (y <= playerY)
                        continue; // skip this Y level

                    if (Math.Sqrt((x - playerX) * (x - playerX) + (y - playerY) * (y - playerY)) <= 32)
                    {
                        bool positionOccupied = Instance.GetLevel().children.Any(obj => obj.Position.X == x && obj.Position.Y == y);

                        if (!positionOccupied)
                        {
                            SolidObject obj = new SolidObject(
                                new Point(x, y),
                                new Size(gridSize, gridSize),
                                "assets\\blocks\\dirt.png",
                                new List<string>() { "Breakable" }
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                    }
                }
            }
        }

        // call the base keypress function
        base.KeyPress(e, held, repeat);
    }

    protected override void OnUpdate(RenderContext ctx)
    {
        // clear last frame using black
        ctx.Clear(Color.Black);

        // display fps in the top right corner
        SizeF size = ctx.MeasureText("FPS: " + GameFPS, 16, "Arial");
        ctx.DrawText("FPS: " + GameFPS, new Point((int)(Width - size.Width - 10), 30), Color.Green, 16, "Arial");

        // update physics
        Instance.GetLocalPlayer().Update(this);

        foreach (SolidObject obj in Instance.GetLevel().children)
        {
            if (Instance.GetLocalPlayer().ResolveCollisionWith(obj))
            {
                // collided with the object so lets check the tags to see what we should do
                if (obj.Tags.Contains("JumpPad"))
                    Instance.GetLocalPlayer().ApplyKnockback(new Point(0, -15), 4);
            }
        }

        // player & scene real quick
        Instance.GetLevel().Draw(ctx);
        Instance.GetLevel().Update(this);
        Instance.GetLocalPlayer().Draw(ctx);

        // some drawings
        //ctx.DrawTriangle(new Point(10, 10), new Point(10, 100), new Point(100, 100), Color.Red, false);
        //ctx.DrawTriangle(new Point(110, 10), new Point(110, 100), new Point(200, 100), Color.Red, true);

        //ctx.DrawRectangle(new Point(10, 110), new Size(90, 90), Color.Red, false);
        //ctx.DrawRectangle(new Point(110, 110), new Size(90, 90), Color.Red, true);

        // end the frame & draw all the remaining objects to the screen
        ctx.EndFrame();
    }
}