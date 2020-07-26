using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshwallControl : MonoBehaviour
{
    Transform sprite;
    public float xShake = 0.1f;
    public float yShake = 0.05f;
    public float rbSpeed = 5f;
    public float acceleration = 0.5f;
    public float maxSpeed = 5f;
    public GameObject player;
    Rigidbody2D rb;
    bool stopped = false;
    void Start()
    {
        sprite = transform.Find("Sprite");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped == false)
        {
            float x = Random.Range(-1f, 1f) * xShake;
            float y = Random.Range(-1f, 1f) * yShake;

            sprite.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

            rb.velocity = new Vector2(0, rbSpeed);
            if (rbSpeed <= maxSpeed)
            {
                rbSpeed += acceleration * Time.deltaTime;
            }
        }
        if (player.transform.position.y - transform.transform.position.y <= -2)
        {
            stopped = true;
            rb.velocity = new Vector2(0, 0);
        }
    }
    public void catchUp(float y)
    {
        if (transform.position.y < y)
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.transform.GetComponent<PlayerControl>().Die();
        }
    }
    public bool isStopped() {
        return stopped;
    }
}
