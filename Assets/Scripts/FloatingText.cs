using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void SetText(int score)
    {
        GetComponentInChildren<TextMesh>().text = score.ToString();
    }
}
