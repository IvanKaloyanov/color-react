using UnityEngine;

public class BlockPrefab : MonoBehaviour
{
    // Block gameObject used as component in the engine

    public int Value { get; set; } // Value for comparison

    public void OnBlockPrefabClicked()
    {
        // If the block is clicked it passes itself to the GameController's method onBlockClicked
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        GameplayController controller = controllerInstance.GetComponent<GameplayController>();
        controller.onBlockClicked(this);
    }
}
