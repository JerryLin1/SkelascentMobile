using System.Collections;
using System.Collections.Generic;
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
    float checkRadius = 0.1f;
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
    bool landed = true;
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
            if (landed == false)
            {
                GameObject impactInstance = Instantiate(impactParticlePrefab, feetPos.position, Quaternion.identity);
                impactInstance.transform.Rotate(10f, 0, 0, Space.Self);
                landed = true;
            }
            rb.gravityScale = normalGravity;
            animator.SetFloat("yVelocity", 0);
        }
        else
        {
            landed = false;
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
        if (hAxis != 0)
        {
            transform.eulerAngles = (hAxis > 0) ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
            animator.SetBool("moving", (!isGrounded) ? false : true);
        }
        else
        {
            animator.SetBool("moving", false);
        }

        if (isGrounded == true && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            GameObject impactInstance = Instantiate(impactParticlePrefab, feetPos.position, Quaternion.identity);
            impactInstance.transform.Rotate(10f, 0, 0, Space.Self);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isJumping == true)
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

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("die");
        }
    }
    public void addBone() {
        bones++;
    }
}
