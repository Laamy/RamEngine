using System.Drawing;

public class ClientInstance
{
    // define the player class
    private Player LocalPlayer = new Player(new Point(100, 0), new Size(26, 32), Color.Red);

    // define the level class
    private Level Level = new Level();

    // get localplayer & get/set level functions
    public Player GetLocalPlayer() => LocalPlayer;

    public Level GetLevel() => Level;
    public void SetLevel(Level level) => Level = level;
}