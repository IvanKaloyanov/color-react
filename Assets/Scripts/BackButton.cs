using UnityEngine;

public class BackButton : MonoBehaviour {

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

    void Start () {
        // Initialize the controller
        GameObject controllerInstance = GameObject.FindGameObjectWithTag(TagsConst.GameController);
        Controller = controllerInstance.GetComponent<GameplayController>();
    }
	
	void Update () {
        // Call ManageBack method if 'back/escape' is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Controller.ManageBack();
        }
    }
}
