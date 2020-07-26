using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadNextLevel : MonoBehaviour
{
    public Animator transition; 

    public void nextLevel() {
        if (SceneManager.GetActiveScene().name == "LevelGeneration") {
            StartCoroutine(fade("MainMenu"));
        } else {
            StartCoroutine(fade("LevelGeneration"));
        }
    }
    public IEnumerator fade(string scene) {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }
}
