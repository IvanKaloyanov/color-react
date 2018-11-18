using System.Collections.Generic;

public class Level  {
    // The representation of a single level.

    private List<Block> blocks; // Blocks for the current level
    private int levelValue; // The center block value
    private int timer; // Timer

    public Level()
    {
        blocks = new List<Block>();
    }

    public List<Block> Blocks
    {
        get
        {
            return this.blocks;
        }

        set
        {
            this.blocks = value;
        }
    }

    public int LevelValue
    {
        get
        {
            return levelValue;
        }

        set
        {
            levelValue = value;
        }
    }
}
