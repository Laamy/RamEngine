using NLua;

using System;
using System.Collections.Generic;
using System.Drawing;

public class ScriptContext
{
    public Lua global = new Lua(); // main state for other states to inherit

    public void InitState()
    {
        // init scripting engine services
        GameInstance game = new GameInstance();

        // first lets expose enums
        global["BlockType"] = typeof(BlockType);

        // now lets expose the main classes like SolidObject..
        global.RegisterFunction("SolidObject", null, typeof(SolidObject).GetConstructor(Type.EmptyTypes)); // SolidObject
        global.RegisterFunction("Point", null, GetType().GetMethod("LUA_Point")); // Point
        global.RegisterFunction("Size", null, GetType().GetMethod("LUA_Size")); // Size
        global.RegisterFunction("Tags", null, GetType().GetMethod("LUA_Tags")); // List<string> => renamed to tags

        // now lets expose custom things like "game/Game"
        global["Game"] = game;
        global["TerrainGen"] = typeof(TerrainGen);
    }

    public Point LUA_Point(int x = 0, int y = 0) => new Point(x, y); // point constructor
    public Size LUA_Size(int x = 0, int y = 0) => new Size(x, y); // size constructor
    public List<string> LUA_Tags(string[] tags) => new List<string>(tags);
}