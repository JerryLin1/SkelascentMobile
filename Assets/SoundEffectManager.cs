using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;



public class SoundEffectManager : MonoBehaviour
{

    public Sprite mutedButton;
    private Sprite defaultButton;
    // Start is called before the first frame update
    void Start()
    {
        
        defaultButton = transform.GetComponent<Image>().sprite;
        if (transform.name == "Mute Sound Effects") transform.GetComponent<Image>().sprite = (PlayerPrefs.GetInt("muteSounds") == 0) ? defaultButton : mutedButton;
        if (transform.name == "Mute Music")         transform.GetComponent<Image>().sprite = (PlayerPrefs.GetInt("muteMusic") == 0) ? defaultButton : mutedButton;
    }

    public void ToggleSoundEffects() {
        PlayerPrefs.SetInt("muteSounds", PlayerPrefs.GetInt("muteSounds") == 0 ? 1 : 0);
        transform.GetComponent<Image>().sprite = (PlayerPrefs.GetInt("muteSounds") == 0) ? defaultButton : mutedButton;
    }

    public void ToggleMusic() {
        PlayerPrefs.SetInt("muteMusic", PlayerPrefs.GetInt("muteMusic") == 0 ? 1 : 0);
        transform.GetComponent<Image>().sprite = (PlayerPrefs.GetInt("muteMusic") == 0) ? defaultButton : mutedButton;

    }
}
