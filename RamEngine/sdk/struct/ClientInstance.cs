using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

public class ClientInstance
{
    // define the player class
    private Player LocalPlayer = new Player(new Point(100, 0), new Size(26, 32), Color.Red);

    // define the level class
    private Level Level = new Level();

    /// <summary>
    /// Get the local player
    /// </summary>
    public Player GetLocalPlayer() => LocalPlayer;

    /// <summary>
    /// Get the level
    /// </summary>
    public Level GetLevel() => Level;

    /// <summary>
    /// Set the level (Deprecated)
    /// </summary>
    [Obsolete("SetLevel is deprecated, please dont try set the level. instead clear it then repopulate its children")]
    public void SetLevel(Level level) => Level = level;
}