using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlatformCollisionHandler : MonoBehaviour
{
    private Transform player;
    private Transform bonePrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "LevelGeneration") {
            player = GameObject.Find("Player").transform;
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Find("Feet").transform.position.y < transform.position.y) {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponentInParent<CompositeCollider2D>());
        } else {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponentInParent<CompositeCollider2D>(), false);
        }
        
    }

    
}
