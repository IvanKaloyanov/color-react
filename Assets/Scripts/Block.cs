using UnityEngine;

public class Block
{
    // The Block class is used to represent the rectangles during gameplay

    public float Height { get; set; } // Height of the block
    public float Width { get; set; } // Width of the block
    public int Value { get; set; } // Unique value for each block
    public Vector3 Possition { get; set; } // Possition on the screen
    public Sprite Background { get; set; } // Background color
}
