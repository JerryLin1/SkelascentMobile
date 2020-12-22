using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeAimMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }
    public void Toggle() {
        if (PlayerPrefs.GetInt("usingAimJoystick", 0) == 0) {
            PlayerPrefs.SetInt("usingAimJoystick", 1);
        }
        else {
            PlayerPrefs.SetInt("usingAimJoystick", 0);
        }
        UpdateText();
    }
    void UpdateText() {
        if (PlayerPrefs.GetInt("usingAimJoystick", 0) == 0) {
            GetComponentInChildren<TextMeshProUGUI>().text = "Joystick";
        }
        else {
            GetComponentInChildren<TextMeshProUGUI>().text = "Screen";
        }
    }
}
