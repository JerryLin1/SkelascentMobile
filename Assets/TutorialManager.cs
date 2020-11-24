using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    int index = 0;
    GameObject child;

    public void NextImage() {
        transform.GetChild(index).gameObject.SetActive(false);

        index++;
        if (index == 3) {
            index = 0;
        }

        transform.GetChild(index).gameObject.SetActive(true);
    }

    public void PreviousImage() {
        transform.GetChild(index).gameObject.SetActive(false);

        index--;
        if (index == -1) {
            index = 2;
        }

        transform.GetChild(index).gameObject.SetActive(true);
    }
}
