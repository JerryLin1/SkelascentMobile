using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class hudControl : MonoBehaviour
{
    public PlayerControl pc;
    TextMeshProUGUI scoreDisplay;
    GameObject boneCounter;
    int score;
    int playerBones = 0;
    int updatedBones;
    void Start()
    {
        scoreDisplay = transform.Find("Score").Find("Text").GetComponent<TextMeshProUGUI>();
        boneCounter = transform.Find("Bone Counter").gameObject;
        updateBoneCount(playerBones);
    }
    void Update()
    {
        score = pc.getScore();
        scoreDisplay.text = "Score: " + score.ToString();
        updatedBones = pc.getBones();
        if (playerBones != updatedBones)
        {
            updateBoneCount(updatedBones);
            playerBones = updatedBones;
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
}
