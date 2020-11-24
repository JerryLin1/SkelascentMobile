using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonControl : MonoBehaviour
{
    float movementSpeed = 2f;
    bool isGrounded;
    float checkRadius = 0.3f;
    public LayerMask groundLayer;
    public GameObject deathParticlePrefab;
    Rigidbody2D rb;
    Animator animator;
    Transform feetPos;
    Transform groundCheckPos;
    int direction;
    AudioManager audioManager;
    void Start()
    {
        animator = GetComponent<Animator>();
        feetPos = transform.Find("Feet");
        groundCheckPos = transform.Find("GroundCheck");
        rb = GetComponent<Rigidbody2D>();
        direction = Random.Range(0, 1);
        if (direction == 0) direction = -1;
        transform.eulerAngles = (direction > 0 ) ? new Vector3(0, 180, 0) : new Vector3(0,0,0);
        audioManager = GetComponent<AudioManager>();
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);
        if (isGrounded == true) {
            rb.velocity = new Vector2(direction * movementSpeed, rb.velocity.y);
            if (!Physics2D.OverlapCircle(groundCheckPos.position, checkRadius, groundLayer)) {
                turnAround();
            }
        }
        else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.tag == "Bone") {
            GameObject.Find("Player").GetComponent<PlayerControl>().demonKilled(transform);
            die();
        }
    }

    public void turnAround() {
        direction *= -1;
        transform.eulerAngles = (direction > 0 ) ? new Vector3(0, 180, 0) : new Vector3(0,0,0);
    }
    public void die () {        
        Instantiate (deathParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
