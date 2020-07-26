using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapControl : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            Debug.Log("you're dead from trap!!!! XD");
            other.GetComponent<PlayerControl>().Die();
        }
    }
}
