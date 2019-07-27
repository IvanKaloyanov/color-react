using UnityEngine;

public class QuitButton : MonoBehaviour {

    private GameplayController controller;

    void Start()
    {
        // Initialize the controller
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        controller = controllerInstance.GetComponent<GameplayController>();
    }

    public void OnQuitButtonClicked() {
        // Call controller's OpenQuitPopup method when BackButton is pressed
        controller.OpenQuitPopup();		
	}
}
