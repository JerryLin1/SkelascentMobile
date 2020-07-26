using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneControl : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float boneXVelocity = 10;
    public GameObject deathParticlePrefab;
    void Awake() {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.angularVelocity = 500f;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void initialVelocity (float playerRotation) {
        if (playerRotation == 180) {
            rb.velocity = new Vector2(boneXVelocity, 30);
        }
        else if (playerRotation == 0) {
            rb.velocity = new Vector2(-boneXVelocity, 30);
        }
    }
    void OnCollisionEnter2D(Collision2D other) {
        Instantiate (deathParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public Collider2D GetCollider2D() {
        return col;
    }
}
