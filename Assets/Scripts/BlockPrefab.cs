using UnityEngine;

public class BlockPrefab : MonoBehaviour
{
    // Block gameObject used as component in the engine

    private int value; // Value for comparison

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

    public void OnBlockPrefabClicked()
    {
        // If the block is clicked it passes itself to the GameController's method onBlockClicked
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        GameplayController controller = controllerInstance.GetComponent<GameplayController>();
        controller.onBlockClicked(this);
    }
}
