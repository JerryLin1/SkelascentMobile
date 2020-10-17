using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshballControl : MonoBehaviour
{

    private Transform player;
    private float speed = 1.5f;

    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
    float amplitude = 0.2f;
    float frequency = 1f;

    bool moveTowardsPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        player = GameObject.Find("Player").transform;
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        // Move towards player
        if (Vector2.Distance(transform.position, player.position) < 3f || moveTowardsPlayer) {
            moveTowardsPlayer = true;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.eulerAngles = (player.position.x > transform.position.x) ? new Vector3(0, 180, 0) : new Vector3(0,0,0);
        } 
        // Float
        else {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
    
            transform.position = tempPos;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            other.GetComponent<PlayerControl>().Die();
        }
    }


}
