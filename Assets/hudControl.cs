using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 using UnityEngine.SceneManagement;

public class hudControl : MonoBehaviour
{
    public PlayerControl pc;

    TextMeshProUGUI scoreDisplay;
    GameObject boneCounter;
    GameObject gameOverScreen;
    GameObject gameOverStats;
    GameObject pauseMenu;
    GameObject darkener;
    TextMeshProUGUI highScoreDisplay;
    int score;
    int playerBones = 0;
    int updatedBones;
    int highScore = ScoreKeeper.highScore;

    void Start()
    {
        Time.timeScale = 1;
        gameOverScreen = transform.Find("GameOver").gameObject;
        gameOverStats = gameOverScreen.transform.Find("Stats").gameObject;
        gameOverScreen.SetActive(false);
        scoreDisplay = transform.Find("Score").Find("Text").GetComponent<TextMeshProUGUI>();
        highScoreDisplay = transform.Find("High Score").Find("Text").GetComponent<TextMeshProUGUI>();
        boneCounter = transform.Find("Bone Counter").gameObject;
        darkener = transform.Find("Darkener").gameObject;
        pauseMenu = transform.Find("PauseMenu").gameObject;
        pauseMenu.SetActive(false);
        updateBoneCount(playerBones);
    }
    void Update()
    {
        score = pc.getScore();
        scoreDisplay.text = "<color=#EEEEEE>Score: " + score.ToString() + "</color>";
        if (score > highScore) {
            highScoreDisplay.text = "<color=#EEEEEE>High Score: " + score.ToString() + "</color>";
            ScoreKeeper.highScore = score;
        } else {
            highScoreDisplay.text = "<color=#EEEEEE>High Score: " + highScore.ToString()+ "</color>";
        }
        
        updatedBones = pc.getBones();
        if (playerBones != updatedBones)
        {
            updateBoneCount(updatedBones);
            playerBones = updatedBones;
        }
        if (gameOverScreen.activeSelf == false && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))) {
            if (pauseMenu.activeSelf == false) pause();
            else unpause();
        }
    }
    public void updateBoneCount(int bones)
    {
        for (int i = 0; i < 6; i++)
        {
            boneCounter.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < bones; i++)
        {
            if (i > 4)
            {
                boneCounter.transform.GetChild(5).gameObject.SetActive(true);
                boneCounter.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "+" + (bones - 5);
            }
            else
                boneCounter.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void enableGameOverScreen() {
        gameOverScreen.SetActive(true);
        darkener.SetActive(true);
        int bonesCollected = pc.getBonesCollected();
        gameOverStats.transform.Find("BonesCollected").GetComponent<TextMeshProUGUI>().text = "<color=#616161>Bones collected: "+ bonesCollected +"</color>";
        int killCount = pc.getKillCount();
        gameOverStats.transform.Find("KillCount").GetComponent<TextMeshProUGUI>().text = "<color=#44212F>Enemies defeated: "+ killCount +"</color>";
        int accuracy = pc.getAccuracy();
        gameOverStats.transform.Find("Accuracy").GetComponent<TextMeshProUGUI>().text = "<color=#3A5339>Bone accuracy: "+ accuracy +"%</color>";
        gameOverStats.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "<color=#323866>Score: "+ score +"</color>";
    }
    public void pause() {
        Time.timeScale = 0;
        darkener.SetActive(true);
        pauseMenu.SetActive(true);
    }
    public void unpause() {
        Time.timeScale = 1;
        darkener.SetActive(false);
        pauseMenu.SetActive(false);
    }
    public void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void returnToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

}
