using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringNewRoom : MonoBehaviour
{

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            transform.parent.GetComponent<SpawnNextRoom>().spawnNextRoom = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.name == "Player") {

            if (other.transform.GetComponent<Rigidbody2D>().velocity.y < 0) {
                // Game over, bring to score screen 
            } else {
                transform.parent.GetComponent<SpawnNextRoom>().leavingRoom = true;

            }

        }
    }

}
