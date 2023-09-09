using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class BytecodeEngine : GameEngine
{
    public BytecodeEngine()
    {
        // setup extra window properties
        Text = "BytecodeEngine";

        // disable resizing
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        
        // setup menustrip
        MenuStrip menu = new MenuStrip();

        // cretae file & exit button in file
        ToolStripMenuItem file = new ToolStripMenuItem("File");
        file.DropDownItems.Add("Exit", null, (s, e) => Close());
        menu.Items.Add(file);

        Controls.Add(menu);

        //loop over all 4 window border walls and add solid objects to stop players from escaping them

        // floor
        //Instance.GetLevel().children.Add(new SolidObject( new Point(0, Height - (menu.Size.Height * 2)), new Size(Width, 10), Color.White ));

        // world stuff
        int blockSize = 20;

        BlockType[][] world = TerrainGen.GenerateWorld(Width/ blockSize, Height/ blockSize, 7, 0.1f);

        // foreach layer
        int layerCount = 100;
        foreach (BlockType[] layer in world)
        {
            int blockCount = 0;
            // foreach terrain block
            foreach (BlockType block in layer)
            {
                switch(block)
                {
                    case BlockType.Stone:
                        {
                            // stone

                            SolidObject obj = new SolidObject(
                                new Point(blockCount, layerCount),
                                new Size(blockSize, blockSize),
                                "assets\\blocks\\stone.png",
                                new List<string>() { "Breakable" }
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                    case BlockType.Grass:
                        {
                            // grass

                            SolidObject obj = new SolidObject(
                                new Point(blockCount, layerCount),
                                new Size(blockSize, blockSize),
                                "assets\\blocks\\grass.png",
                                new List<string>() { "Breakable" }
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                    case BlockType.Dirt:
                        {
                            // dirt

                            SolidObject obj = new SolidObject(
                                new Point(blockCount, layerCount),
                                new Size(blockSize, blockSize),
                                "assets\\blocks\\dirt.png",
                                new List<string>() { "Breakable" }
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                }

                blockCount += blockSize;
            }
            layerCount += blockSize;
        }

        // jumppad
        Instance.GetLevel().children.Add(new SolidObject(
            new Point(50, Height - (menu.Size.Height * 2) - 10),
            new Size(30, 11),
            Color.Red,
            new List<string> { "JumpPad" }
        ));

        // start the game winodw & engine
        Start();
    }

    protected override void KeyPress(Keys e, bool held, bool repeat)
    {
        if (e == Keys.Space)
            Instance.GetLocalPlayer().JumpFromGround();

        if (e == Keys.S)
        {
            List<SolidObject> objectsToRemove = new List<SolidObject>();

            foreach (SolidObject obj in Instance.GetLevel().children)
            {
                if (obj.Tags.Contains("Breakable") && obj.DistanceToPoint(Instance.GetLocalPlayer().Position) < 20*2)
                {
                    objectsToRemove.Add(obj);
                }
            }

            Instance.GetLevel().RemoveBulk(objectsToRemove);
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