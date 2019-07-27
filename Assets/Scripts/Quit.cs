using UnityEngine;

public class Quit : MonoBehaviour {

    private GameplayController controller;

    void Start()
    {
        // Initialize the controller
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        controller = controllerInstance.GetComponent<GameplayController>();
    }

    public void OnQuitClicked()
    {
        // Quit from the App
        Application.Quit();
    }

    public void OnRefuseQuitClicked()
    {
        // Reject quiting
        controller.CloseQuitPopup();
    }
}
