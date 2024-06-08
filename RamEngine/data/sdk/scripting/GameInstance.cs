public class GameInstance
{
    /// <summary>
    /// The version of the game that the scripts are running on
    /// </summary>
    public int[] Version { get; set; } = { 1, 0, 0, 0 };

    /// <summary>
    /// The game working space
    /// </summary>
    public Level Workspace { get; } = GameEngine.Instance.GetLevel();
}