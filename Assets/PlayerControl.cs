using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float movementSpeed = 7f;
    float jumpForce = 20f;
    bool isGrounded;
    Transform feetPos;
    Transform boneSourcePos;
    float checkRadius = 0.05f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public GameObject bonePrefab;
    public GameObject impactParticlePrefab;
    private float jumpTimeCounter;
    private bool isJumping;
    float jumpTime = 0.15f;
    float hAxis;
    float normalGravity = 5f;
    float fallingGravity = 8f;
    float boneCd = 1f;
    float boneCdTimer;
    int bones = 0;
    float coyoteTime = 0.2f;
    float coyoteTimeTimer;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        feetPos = transform.Find("Feet");
        boneSourcePos = transform.Find("BoneSource");
        animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(hAxis * movementSpeed, rb.velocity.y);
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);
        if (isGrounded == true)
        {
            rb.gravityScale = normalGravity;
            animator.SetFloat("yVelocity", 0);
        }
        else
        {
            coyoteTimeTimer = coyoteTime;
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        // Rotate player and play right animation
        if (hAxis != 0){
            transform.eulerAngles = (hAxis > 0) ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
            animator.SetBool("moving", (!isGrounded) ? false : true);
        } else {
            animator.SetBool("moving", false);
        }

        // If player is on the ground and they jump
        if (Math.Abs(rb.velocity.y) <= 0.01f && (isGrounded == true || coyoteTimeTimer > 0) && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            GameObject impactInstance = Instantiate(impactParticlePrefab, feetPos.position, Quaternion.identity);
            impactInstance.transform.Rotate(10f, 0, 0, Space.Self);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            coyoteTimeTimer = 0;
        }

        // While player is in middle of jump
        if (isJumping == true)
        {

            if (jumpTimeCounter > 0)
            {
                rb.AddForce(new Vector2(0, jumpForce * 0.025f), ForceMode2D.Impulse);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                rb.gravityScale = fallingGravity;
            }
        }

        // After player hits jump
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
            rb.gravityScale = fallingGravity;
        }
        if (Input.GetKeyDown(KeyCode.Space) && boneCdTimer <= 0 && bones > 0)
        {
            GameObject boneInstance = Instantiate(bonePrefab, boneSourcePos.position, Quaternion.identity);
            Physics2D.IgnoreCollision(col, boneInstance.GetComponent<BoneControl>().GetCollider2D(), true);
            boneInstance.GetComponent<BoneControl>().initialVelocity(transform.eulerAngles.y);
            boneCdTimer = boneCd;
            bones--;
        }
        boneCdTimer -= Time.deltaTime;
        coyoteTimeTimer -= Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("die");
        }

        if (col.gameObject.tag == "Platform") {
            Instantiate(impactParticlePrefab, feetPos.position, Quaternion.identity);

        }

    }
    public void addBone()
    {
        bones++;
    }
}
