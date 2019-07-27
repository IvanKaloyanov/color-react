using UnityEngine;

public class MusicController : MonoBehaviour {

    // MusicController responsible to play music
    public static MusicController Instance; // Singelton instance

    [Header("Referances")]
    [SerializeField]
    public AudioSource rightSound; // a sound

    [SerializeField]
    public AudioSource wrongSound; // a sound

    private MusicController()
    {

    }
    void Start () {
        // Instanciate the singelton instance
        if (Instance == null)
        {
            Instance = this;
        }
	}
}
