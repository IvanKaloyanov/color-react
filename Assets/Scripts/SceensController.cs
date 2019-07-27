using UnityEngine;

public class SceensController : MonoBehaviour {

    private GameplayController controller;

    private void Start()
    {
        // Initialize the controller
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        controller = controllerInstance.GetComponent<GameplayController>();
    }

    public void OnRestartButton()
    {
        // Call controller's RestartGame method when RestartButton is pressed
        controller.RestartGame();
    }
}
