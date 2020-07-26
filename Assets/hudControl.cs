using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class hudControl : MonoBehaviour
{
    public PlayerControl pc;
    TextMeshProUGUI scoreDisplay;
    int score;
    void Start()
    {
        scoreDisplay = transform.Find("Score").Find("Text").GetComponent<TextMeshProUGUI>();
    }
    void Update() {
        score = pc.getScore();
        scoreDisplay.text = score.ToString();
    }
}
