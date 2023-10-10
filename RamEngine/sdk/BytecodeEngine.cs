using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class BytecodeEngine : GameEngine
{
    public BytecodeEngine()
    {
        // block highlight
        Instance.GetLevel().children.Add(BlockHighLight);

        // world stuff
        int blockSize = 10;
 
        BlockType[][] world = TerrainGen.GenerateWorld(Width / blockSize, (int)((Height) / blockSize), 8, 0.03f);

        // foreach layer
        int layerCount = 100;
        foreach (BlockType[] layer in world)
        {
            int blockCount = 0;
            // foreach terrain block
            foreach (BlockType block in layer)
            {
                switch (block)
                {
                    case BlockType.Stone:
                        {
                            // stone

                            SolidObject obj = new SolidObject(
                                new Vector2(blockCount, layerCount),
                                new Vector2(blockSize, blockSize),
                                Color4.Gray,
                                "assets\\blocks\\stone.png"
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                    case BlockType.Grass:
                        {
                            // grass

                            SolidObject obj = new SolidObject(
                                new Vector2(blockCount, layerCount),
                                new Vector2(blockSize, blockSize),
                                Color4.Green,
                                "assets\\blocks\\grass.png"
                            );

                            Instance.GetLevel().children.Add(obj);
                        }
                        break;
                    case BlockType.Dirt:
                        {
                            // dirt

                            SolidObject obj = new SolidObject(
                                new Vector2(blockCount, layerCount),
                                new Vector2(blockSize, blockSize),
                                Color4.Brown,
                                "assets\\blocks\\dirt.png"
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
        //Instance.GetLevel().children.Add(new SolidObject(
        //    new Point(50, Height - (menu.Size.Height * 2) - 10),
        //    new Size(30, 11),
        //    Color.Red,
        //    new List<string> { "JumpPad" }
        //));

        // start the game window & engine
        Start();
    }

    public SolidObject BlockHighLight = new SolidObject(new Vector2(0, 0), new Vector2(0, 0), Color.Yellow);

    //protected override void MouseMoveClick(Point point, MouseButtons button)
    //{
    //    if (button == MouseButtons.Left)
    //    {
    //        Raycast ray = Instance.GetLevel().CastRay(Instance.GetLocalPlayer().Center, point);
    //        if (ray != null)
    //        {
    //            if (ray.HasCollided && ray.Dist < 100)
    //            {
    //                Instance.GetLevel().Remove(ray.Hit);
    //            }
    //            else
    //            {
    //                // log ray information
    //                Console.WriteLine("Dist: 0x" + ray.Dist + " " + ray.Position + " " + ray.HasCollided);
    //            }
    //        }
    //    }
    //}

    //protected override void KeyPress(Keys e, bool held, bool repeat)
    //{
    //    if (e == Keys.Space)
    //        Instance.GetLocalPlayer().JumpFromGround();

    //    if (e == Keys.G)
    //    {
    //        // create a particle
    //        SolidObject obj = ParticleHandler.CreateParticle(5, 5, "assets\\blocks\\dirt.png", 100, new Point(0, 0));
    //        obj.Position = new Point(300, 300);

    //        Instance.GetLevel().children.Add(obj);
    //    }

    //    // debug key
    //    if (e == Keys.S)
    //    {
    //        List<SolidObject> objectsToRemove = new List<SolidObject>();

    //        foreach (SolidObject obj in Instance.GetLevel().children)
    //        {
    //            if (obj.Tags.Contains("Breakable") && obj.DistanceTo(Instance.GetLocalPlayer().Center) < 32)
    //            {
    //                objectsToRemove.Add(obj);
    //            }
    //        }

    //        Instance.GetLevel().RemoveBulk(objectsToRemove);
    //    }

    //    // call the base keypress function
    //    base.KeyPress(e, held, repeat);
    //}

    protected override void OnUpdate(RenderContext ctx)
    {
        // clear last frame using black
        ctx.Clear(Color.Black);
        ctx.BeginFrame();

        // display fps in the top right corner
        //SizeF size = ctx.MeasureText("FPS: " + GameFPS, 16, "Arial");
        //ctx.DrawText("FPS: " + GameFPS, new Point((int)(Width - size.Width - 10), 30), Color.Green, 16, "Arial");

        // update physics
        Instance.GetLocalPlayer().Update(this);

        foreach (SolidObject obj in Instance.GetLevel().children)
        {
            if (Instance.GetLocalPlayer().ResolveCollisionWith(obj))
            {
                // collided with object event triggered here
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