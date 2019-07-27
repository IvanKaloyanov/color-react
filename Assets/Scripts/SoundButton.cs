using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {

    // Sound menu controller
    [Header("Referances")]
    [SerializeField]
    public Image musicIcon;     // icon

    [SerializeField]
    public Image musicOnIcon;   // onImage

    [SerializeField]
    public Image musicOffIcon;  // offImage

    private bool mute;          // mute flag

    private void Start()
    {
        // Set the user's settings
        mute = bool.Parse(PlayerPrefs.GetString(PrefsConst.Mute, false.ToString()));
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
        PlayerPrefs.SetString(PrefsConst.Mute, mute.ToString());
    }
}
