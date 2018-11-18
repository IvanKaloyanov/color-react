using UnityEngine;

public class MusicController : MonoBehaviour {
    // MusicController responsible to play music

    public static MusicController Instance; // Singelton instance
    public AudioSource rightSound; // a sound
    public AudioSource wrongSound; // a sound

    void Start () {
        // Instanciate the singelton instance
        if (Instance == null)
        {
            Instance = this;
        }
	}
}
