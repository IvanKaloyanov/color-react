using System.Collections;
using UnityEngine;

public class DropdownController : MonoBehaviour {

    private const int scaleSpeed = 12; // The speed for scrolling down

    public GameObject menu; // The menu itself
    private Vector3 scale;
    public bool isOpen;

	void Start ()
    {   
        // Init the default values
        scale = menu.transform.localScale;
        isOpen = false;
    }

    public void OnDropdownPressed()
    {
        // Hide or scrolldown the menu on click
        isOpen = !isOpen;
        menu.SetActive(isOpen);
        StartCoroutine(ManageDropdown());
    }

    private IEnumerator ManageDropdown()
    {
        // Scale the menu over the time.
        do
        {
            scale.y = Mathf.Lerp(scale.y, isOpen ? 1 : -0.5f, Time.deltaTime * scaleSpeed);
            menu.transform.localScale = scale;
            Debug.Log(scale.y.ToString());
            yield return null;
        } while (scale.y < 0.98f && scale.y > -0.5f);
    }
}
