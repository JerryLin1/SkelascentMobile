using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float movementSpeed = 5f;
    float jumpForce = 18f;
    bool isGrounded;
    Transform feetPos;
    Transform boneSourcePos;
    float checkRadius = 0.1f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public GameObject bonePrefab;
    public GameObject impactParticlePrefab;
    public GameObject deathParticlePrefab;
    private float jumpTimeCounter;
    private bool isJumping;
    float jumpTime = 0.2f;
    float hAxis;
    float normalGravity = 5f;
    float fallingGravity = 8f;
    float boneCd = 1f;
    float boneCdTimer;
    int bones = 0;
    float coyoteTime = 0.3f;
    float coyoteTimeTimer;
    Animator animator;
    int bonusScore = 0;
    int maxScore = 0;
    bool gameOver = false;
    int killCount = 0;
    int bonesThrownCount = 0;
    int bonesCollectedCount = 0;
    Vector2 direction;
    Vector3 mousePos;
    AudioManager audioManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        feetPos = transform.Find("Feet");
        boneSourcePos = transform.Find("BoneSource");
        animator = transform.Find("Sprite").GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameOver)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(hAxis * movementSpeed, rb.velocity.y);
    
        }
    }
    void Update()
    {
        if (!gameOver) Move();
        else if (Input.GetKeyDown(KeyCode.Space)) GameObject.Find("Ui").GetComponent<hudControl>().restart();   
    }

    void Move()
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
        if (hAxis != 0)
        {
            transform.eulerAngles = (hAxis > 0) ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
            animator.SetBool("moving", (!isGrounded) ? false : true);
        }
        else
        {
            animator.SetBool("moving", false);
        }

        // If player is on the ground and they jump
        if (Math.Abs(rb.velocity.y) <= 0.1f && (isGrounded == true || coyoteTimeTimer > 0) && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            audioManager.Play("Jump");
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
        if (Input.GetMouseButtonDown(0) && boneCdTimer <= 0 && bones > 0)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
            direction.Normalize();
            bonesThrownCount++;
            audioManager.Play("ThrowBone");
            GameObject boneInstance = Instantiate(bonePrefab, boneSourcePos.position, Quaternion.identity);
            Physics2D.IgnoreCollision(col, boneInstance.GetComponent<BoneControl>().GetCollider2D(), true);
            boneInstance.GetComponent<BoneControl>().initialVelocity(direction);
            boneCdTimer = boneCd;
            bones--;
        } else if (Input.GetMouseButtonDown(0) && boneCdTimer <= 0 && bones == 0) {
            audioManager.Play("OutofBones");
            boneCdTimer = boneCd;
            GameObject.Find("Ui").GetComponent<hudControl>().outOfBones();
        }
        boneCdTimer -= Time.deltaTime;
        coyoteTimeTimer -= Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Die();
        }

        if (col.gameObject.tag == "Platform")
        {
            Instantiate(impactParticlePrefab, feetPos.position, Quaternion.identity);

        }
    }
    public void addBone()
    {
        bonesCollectedCount++;
        audioManager.Play("PickupBone");
        bones++;
        addScore(20);
    }
    public int getBones() { return bones; }
    public void demonKilled()
    {
        audioManager.Play("KillDemon");
        killCount++;
        addScore(50);
    }
    public void boneImpact()
    {
        audioManager.Play("BoneImpact");
    }
    public void addScore(int score)
    {
        StartCoroutine(Camera.main.GetComponent<CameraControl>().cameraShake(0.1f, 0.5f));
        bonusScore += score;
    }
    public int getBonesCollected() { return bonesCollectedCount; }
    public int getKillCount() { return killCount; }
    public int getAccuracy()
    {
        if (bonesThrownCount == 0) {
            return 0;
        }
        float inaccuracy = (bonesThrownCount - killCount) / (float)bonesThrownCount;
        inaccuracy*=100;
        inaccuracy = 100 - inaccuracy;
        int accuracy = (int) inaccuracy;
        return accuracy;
    }
    public int getScore()
    {
        if ((int)transform.position.y > maxScore)
        {
            maxScore = (int)transform.position.y;
        }
        return maxScore + bonusScore;
    }
    public void Die()
    {
        audioManager.Play("GameOver");
        // reset position
        // transform.position = new Vector3(0, -3, 0);
        Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        gameOver = true;
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("moving", false);
        animator.SetFloat("yVelocity", 0);
        animator.SetBool("death", true);

        // reset player variables
        bones = 0;
        StartCoroutine(GameOver());
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Game over");
        GameObject.Find("Ui").GetComponent<hudControl>().enableGameOverScreen();
    }
}
