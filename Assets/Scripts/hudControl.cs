using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using DG.Tweening;

public class hudControl : MonoBehaviour
{
    public bool usingAimJoystick = false;
    public PlayerControl pc;

    TextMeshProUGUI scoreDisplay;
    GameObject boneCounter;
    GameObject gameOverScreen;
    GameObject gameOverStats;
    GameObject pauseMenu;
    GameObject darkener;
    GameObject mobileControls;
    TextMeshProUGUI highScoreDisplay;
    AdControl adControl;
    SocialControl socialControl;
    int score;
    int playerBones = 0;
    int updatedBones;
    int highScore;

    float noBonesEmphasisTimer = 0;
    float noBonesEmphasisDuration = 0.1f;

    Vector3 gameOverScreenpos1;
    Vector3 gameOverScreenpos2 = new Vector3(0, 0, 0);

    void Start()
    {
        Time.timeScale = 1;

        gameOverScreen = transform.Find("GameOver").gameObject;
        gameOverStats = gameOverScreen.transform.Find("Stats").gameObject;
        gameOverScreenpos1 = gameOverScreen.transform.position;

        scoreDisplay = transform.Find("Score").Find("Text").GetComponent<TextMeshProUGUI>();
        highScoreDisplay = transform.Find("High Score").Find("Text").GetComponent<TextMeshProUGUI>();
        boneCounter = transform.Find("Bone Counter").gameObject;
        darkener = transform.Find("Darkener").gameObject;
        pauseMenu = transform.Find("PauseMenu").gameObject;
        pauseMenu.SetActive(false);
        adControl = GameObject.Find("Advertisement Manager").GetComponent<AdControl>();
        socialControl = GameObject.Find("Social").GetComponent<SocialControl>();
        mobileControls = transform.Find("Mobile Controls").gameObject;
        
        if (usingAimJoystick == false) {
            GameObject aimJoystick = mobileControls.transform.Find("Aim Joystick").gameObject;
            aimJoystick.transform.position = Vector3.zero;
            aimJoystick.transform.localScale = new Vector3(6, 6, 1);
            aimJoystick.transform.Find("Visual").GetComponent<Image>().enabled = false;
            aimJoystick.transform.Find("Handle").GetComponent<Image>().enabled = false;
        }

        highScore = PlayerPrefs.GetInt("Highscore", 0);
        highScoreDisplay.text = "<color=#EEEEEE>High Score: " + highScore.ToString() + "</color>";
        updateBoneCount(playerBones);
    }
    void Update()
    {
        if (noBonesEmphasisTimer > 0)
        {
            noBonesEmphasisTimer -= Time.deltaTime;
            if (noBonesEmphasisTimer <= 0) boneCounter.transform.Find("Out of Bones").gameObject.GetComponent<TextMeshProUGUI>().fontSize -= 5;
        }

        score = pc.getScore();
        scoreDisplay.text = "<color=#EEEEEE>Score: " + score.ToString() + "</color>";
        if (score > highScore)
        {
            highScoreDisplay.text = "<color=#EEEEEE>High Score: " + score.ToString() + "</color>";
            highScore = score;
        }

        updatedBones = pc.getBones();
        if (updatedBones > 0)
        {
            boneCounter.transform.Find("Out of Bones").gameObject.SetActive(false);
        }
        else
        {
            boneCounter.transform.Find("Out of Bones").gameObject.SetActive(true);

        }
        if (playerBones != updatedBones)
        {
            updateBoneCount(updatedBones);
            playerBones = updatedBones;
        }
        if (gameOverScreen.activeSelf == false && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            if (pauseMenu.activeSelf == false) pause();
            else unpause();
        }
    }
    public void updateBoneCount(int bones)
    {
        for (int i = 0; i < 7; i++)
        {
            boneCounter.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 1; i < bones + 1; i++)
        {
            if (i > 5)
            {
                boneCounter.transform.GetChild(6).gameObject.SetActive(true);
                boneCounter.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "+" + (bones - 5);
            }
            else
                boneCounter.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void enableGameOverScreen()
    {
        // enable if u want to test ads
        if (PlayerPrefs.GetInt("AdsEnabled", 0) == 0)
        {
            if (Random.Range(1, 10) <= 3) adControl.ShowInterstitialAd();
        }
        gameOverScreen.transform.DOLocalMove(gameOverScreenpos2, 0.3f);
        Darken(true);
        int bonesCollected = pc.getBonesCollected();
        gameOverStats.transform.Find("BonesCollected").GetComponent<TextMeshProUGUI>().text = "<color=#616161>Bones collected: " + bonesCollected + "</color>";
        int killCount = pc.getKillCount();
        gameOverStats.transform.Find("KillCount").GetComponent<TextMeshProUGUI>().text = "<color=#44212F>Enemies defeated: " + killCount + "</color>";
        int accuracy = pc.getAccuracy();
        gameOverStats.transform.Find("Accuracy").GetComponent<TextMeshProUGUI>().text = "<color=#3A5339>Bone accuracy: " + accuracy + "%</color>";
        gameOverStats.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "<color=#323866>Score: " + score + "</color>";
        socialControl.ReportScore(score);
        if (score > PlayerPrefs.GetInt("Highscore", 0))
            PlayerPrefs.SetInt("Highscore", highScore);
    }
    public void pause()
    {
        HideMobileControls();
        Darken(true);
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    public void unpause()
    {
        ShowMobileControls();
        Darken(false);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void returnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void outOfBones()
    {
        boneCounter.transform.Find("Out of Bones").gameObject.GetComponent<TextMeshProUGUI>().fontSize += 5;
        noBonesEmphasisTimer = noBonesEmphasisDuration;
    }
    public void Darken(bool darken)
    {
        if (darken == true)
            darkener.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0.39f), 0.2f).SetUpdate(true);
        else
            darkener.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.2f).SetUpdate(true);
    }
    public void HideMobileControls() { mobileControls.SetActive(false); }
    public void ShowMobileControls() { mobileControls.SetActive(true); }

}
