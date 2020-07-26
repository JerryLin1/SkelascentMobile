﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneControl : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float boneXVelocity = 15;
    float boneYVelocity = 20;
    public GameObject deathParticlePrefab;
    void Awake() {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.angularVelocity = 500f;
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void initialVelocity (float playerRotation) {
        if (playerRotation == 180) {
            rb.velocity = new Vector2(boneXVelocity, boneYVelocity);
        }
        else if (playerRotation == 0) {
            rb.velocity = new Vector2(-boneXVelocity, boneYVelocity);
        }
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
