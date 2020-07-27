using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneControl : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float boneXVelocity = 15;
    float boneYVelocity = 20;
    public GameObject deathParticlePrefab;
    Vector3 mousePos;
    Vector2 direction;


    void Awake() {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.angularVelocity = 1000f;
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8);
    }
    

    // Update is called once per frame
    void Update()
    {

    }
    public void initialVelocity (Vector2 direction) {
        rb.velocity = direction * 20f;
    }
    void OnCollisionEnter2D(Collision2D other) {
        GameObject.Find("Player").GetComponent<PlayerControl>().boneImpact();
        Instantiate (deathParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public Collider2D GetCollider2D() {
        return col;
    }
}
