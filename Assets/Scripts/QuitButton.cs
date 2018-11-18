using UnityEngine;

public class QuitButton : MonoBehaviour {

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

    void Start()
    {
        // Initialize the controller
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        Controller = controllerInstance.GetComponent<GameplayController>();
    }

    public void OnQuitButtonClicked() {
        // Call controller's OpenQuitPopup method when BackButton is pressed
        Controller.OpenQuitPopup();		
	}
}
