using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    float movementSpeed = 10f;
    float jumpForce = 15f;
    bool isGrounded;
    Transform feetPos;
    float checkRadius = 0.3f;
    public LayerMask groundLayer;
    private float jumpTimeCounter;
    private bool isJumping;
    float jumpTime = 0.15f;
    float hAxis;
    float normalGravity = 5f;
    float fallingGravity = 8f;
    Animator animator;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        feetPos = transform.Find("Feet");
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
        if (isGrounded == true) rb.gravityScale = normalGravity;
        if (hAxis > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            animator.SetBool("moving", true);
        }
        else if (hAxis < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            animator.SetBool("moving", true);
        }
        else {
            animator.SetBool("moving", false);
        }

        if (isGrounded == true && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isJumping == true)
        {
            animator.SetTrigger("jumping");
            if (jumpTimeCounter > 0)
            {
                rb.AddForce(new Vector2(0, jumpForce*0.025f), ForceMode2D.Impulse);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                rb.gravityScale = fallingGravity;
            }
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
            rb.gravityScale = fallingGravity;
        }
    }
}
