using System.Collections.Generic;

public class Level  {

    // The representation of a single level.
    public List<Block> Blocks { get; set; } // Blocks for the current level
    public int LevelValue { get; set; }     // The center block value
    public int Timer { get; set; }          // Timer

    public Level()
    {
        Blocks = new List<Block>();
    }
}
