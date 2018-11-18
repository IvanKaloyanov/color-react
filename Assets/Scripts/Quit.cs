using UnityEngine;

public class Quit : MonoBehaviour {

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

    public void OnQuitClicked()
    {
        // Quit from the App
        Application.Quit();
    }

    public void OnRefuseQuitClicked()
    {
        // Reject quiting
        Controller.CloseQuitPopup();
    }

}
