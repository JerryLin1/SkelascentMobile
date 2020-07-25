using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnIfCollided : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Walls") {
            transform.parent.GetComponent<DemonControl>().turnAround();
        }
    }
}
