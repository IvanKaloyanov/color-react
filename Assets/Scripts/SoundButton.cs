using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {
    // Sound menu controller

    public Image musicIcon; // icon
    public Image musicOnIcon; // onImage
    public Image musicOffIcon; // offImage

    public bool mute;
    private const string ppValue = "mute";

    private void Start()
    {
        // Set the user's settings
        mute = bool.Parse(PlayerPrefs.GetString(ppValue, false.ToString()));
        MusicController.Instance.wrongSound.mute = mute;
        MusicController.Instance.rightSound.mute = mute;
        musicIcon.sprite = mute ? musicOffIcon.sprite : musicOnIcon.sprite;
    }

    public void OnSoundButtonClicked()
    {
        // Turn the sounds 'on' and 'off' on click
        mute = !mute;
        MusicController.Instance.wrongSound.mute = mute;
        MusicController.Instance.rightSound.mute = mute;
        musicIcon.sprite = mute ? musicOffIcon.sprite : musicOnIcon.sprite;
        PlayerPrefs.SetString(ppValue, mute.ToString());
    }
}
