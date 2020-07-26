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
    Rigidbody2D rb;
    void Start()
    {
        sprite = transform.Find("Sprite");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Random.Range(-1f, 1f) * xShake;
        float y = Random.Range(-1f, 1f) * yShake;

        sprite.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

        rb.velocity = new Vector2(0, rbSpeed);
        if (rbSpeed <= maxSpeed) {
            rbSpeed += acceleration * Time.deltaTime;
        }
    }
    public void teleport(float y) {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
