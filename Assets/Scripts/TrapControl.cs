using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapControl : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player" && other.GetComponent<PlayerControl>() != null) {
            other.GetComponent<PlayerControl>().Die();
        }
    }
}
