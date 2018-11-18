using UnityEngine;

public class Block
{
    // The Block class is used to represent the rectangles during gameplay

    private float height; // Size of the block
    private float width; // -- -- -- -- --
    private int value; // Unique value for each block
    private Vector3 possition; // Possition on the screen
    private Sprite background; // Background color

    public float Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public Vector3 Possition
    {
        get
        {
            return possition;
        }

        set
        {
            possition = value;
        }
    }

    public int Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
        }
    }

    public Sprite Background
    {
        get
        {
            return background;
        }

        set
        {
            background = value;
        }
    }
}
