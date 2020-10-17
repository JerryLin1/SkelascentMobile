

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatControl : MonoBehaviour
{

    public GameObject deathParticlePrefab;

    private Transform player;
    private float speed = 1.5f;

    private Vector3 posOffset = new Vector3 ();
    private Vector3 tempPos = new Vector3 ();
    private float amplitude = 0.2f;
    private float frequency = 1f;

    bool moveTowardsPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 9);
        player = GameObject.Find("Player").transform;
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        // Move towards player
        if (Vector2.Distance(transform.position, player.position) < 4f || moveTowardsPlayer) {
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

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Player" && other.gameObject.GetComponent<PlayerControl>() != null) {
            other.gameObject.GetComponent<PlayerControl>().Die();
        }
        if (other.collider.gameObject.tag == "Bone") {
            GameObject.Find("Player").GetComponent<PlayerControl>().demonKilled();
            die();
        }
    }

    void die() {
        Instantiate (deathParticlePrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }


}