using UnityEngine;

public class SceensController : MonoBehaviour {

    private GameplayController controller;

    public GameplayController Controller
    {
        get
        {
            return controller;
        }
        set
        {
            controller = value;
        }
    }

    private void Start()
    {
        // Initialize the controller
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        Controller = controllerInstance.GetComponent<GameplayController>();
    }

    public void OnRestartButton()
    {
        // Call controller's RestartGame method when RestartButton is pressed
        Controller.RestartGame();
    }
}
