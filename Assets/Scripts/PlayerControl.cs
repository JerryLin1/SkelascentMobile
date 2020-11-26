using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Lean.Gui;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float movementSpeed = 5f;
    float jumpForce = 18f;
    bool isGrounded;
    Transform feetPos;
    Transform boneSourcePos;
    float checkRadius = 0.3f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public GameObject bonePrefab;
    public GameObject impactParticlePrefab;
    public GameObject deathParticlePrefab;
    public GameObject floatingTextPrefab;
    private float jumpTimeCounter;
    private bool isJumping;
    float jumpTime = 0.2f;
    float hAxis;
    float normalGravity = 5f;
    float fallingGravity = 8f;
    float boneCd = 0.25f;
    float boneCdTimer;
    int bones = 3;
    float coyoteTime = 0.2f;
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
    hudControl hudControl;
    GameObject mobileControls;
    LeanJoystick joystickMove;
    LeanJoystick joystickAim;
    bool jumpButtonDown = false;
    LineRenderer line;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        feetPos = transform.Find("Feet");
        boneSourcePos = transform.Find("BoneSource").Find("Source");
        animator = transform.Find("Sprite").GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
        hudControl = GameObject.Find("Ui").GetComponent<hudControl>();

        // Find mobile controls
        mobileControls = GameObject.Find("Ui").transform.Find("Mobile Controls").gameObject;
        joystickMove = mobileControls.transform.Find("Movement Joystick").GetComponent<LeanJoystick>();
        joystickAim = mobileControls.transform.Find("Aim Joystick").GetComponent<LeanJoystick>();
        line = transform.Find("AimLine").GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameOver)
        {
            if (joystickMove.ScaledValue.x > 0.5) hAxis = 1;
            else if (joystickMove.ScaledValue.x < -0.5) hAxis = -1;
            else hAxis = 0;

            rb.velocity = new Vector2(hAxis * movementSpeed, rb.velocity.y);
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

        }
    }
    void Update()
    {
        if (!gameOver) Move();
        else if (Input.GetKeyDown(KeyCode.Space)) hudControl.restart();
    }

    void Move()
    {
        bool isGroundBelow = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);
        if (Math.Abs(rb.velocity.y) <= 0.1f && isGroundBelow)
        {
            if (!isGrounded) Instantiate(impactParticlePrefab, feetPos.position, Quaternion.identity);
            isGrounded = true;
        }
        else isGrounded = false;
        if (isGrounded == true)
        {
            coyoteTimeTimer = coyoteTime;
            rb.gravityScale = normalGravity;
            animator.SetFloat("yVelocity", 0);
        }
        else
        {
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        // Rotate player and play right animation
        if (hAxis != 0)
        {
            transform.localRotation = (hAxis > 0) ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
            line.transform.localRotation = (hAxis > 0) ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);

            animator.SetBool("moving", (!isGrounded) ? false : true);
        }
        else
        {
            animator.SetBool("moving", false);
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
    }
    public void addBone()
    {
        bonesCollectedCount++;
        audioManager.Play("PickupBone");
        Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        bones++;
        CreateFloatingText(transform, 20);
        addScore(20);
    }
    public int getBones() { return bones; }
    public void demonKilled(Transform demonTranform)
    {
        audioManager.Play("KillDemon");
        StartCoroutine(Camera.main.GetComponent<CameraControl>().cameraShake(0.05f, 0.5f));
        killCount++;
        CreateFloatingText(demonTranform, 50);
        addScore(50);
    }
    public void boneImpact()
    {
        audioManager.Play("BoneImpact");
    }
    public void CreateFloatingText(Transform transformPos, int score)
    {
        Vector3 withOffset = new Vector3(transformPos.position.x, transformPos.position.y + 0.5f, transformPos.position.z);
        GameObject IfloatingText = Instantiate(floatingTextPrefab, withOffset, Quaternion.identity);
        IfloatingText.GetComponentInChildren<FloatingText>().SetText(score);
    }
    public void addScore(int score)
    {
        bonusScore += score;
    }
    public int getBonesCollected() { return bonesCollectedCount; }
    public int getKillCount() { return killCount; }
    public int getAccuracy()
    {
        if (bonesThrownCount == 0)
        {
            return 0;
        }
        float inaccuracy = (bonesThrownCount - killCount) / (float)bonesThrownCount;
        inaccuracy *= 100;
        inaccuracy = 100 - inaccuracy;
        int accuracy = (int)inaccuracy;
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
        line.enabled = false;
        hudControl.HideMobileControls();
        audioManager.Play("GameOver");
        StartCoroutine(Camera.main.GetComponent<CameraControl>().cameraShake(0.2f, 1f));
        Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        gameOver = true;
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("moving", false);
        animator.SetFloat("yVelocity", 0);
        animator.SetBool("death", true);
        StartCoroutine(GameOver());
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        hudControl.enableGameOverScreen();
    }

    public void JumpButtonDown()
    {
        if (isGrounded == true || coyoteTimeTimer > 0)
        {
            audioManager.Play("Jump");
            GameObject impactInstance = Instantiate(impactParticlePrefab, feetPos.position, Quaternion.identity);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            coyoteTimeTimer = 0;
        }
    }
    public void JumpButtonRelease()
    {
        isJumping = false;
        rb.gravityScale = fallingGravity;
    }

    public void StartAimBone()
    {
        // enable line?
        // line.positionCount = 1;
        // line.SetPosition(0, boneSourcePos.position);
        // Vector2 direction = joystickAim.ScaledValue.normalized;
        // RaycastHit2D rayInfo = Physics2D.Raycast(boneSourcePos.position, direction);
        // line.positionCount++;
        // line.SetPosition(line.positionCount-1, rayInfo.point);
    }

    // this function being called while joystick is being used
    public void AimBone()
    {
        // update line/raycast
        // line.positionCount = 2;
        Vector2 direction = joystickAim.ScaledValue.normalized;
        transform.Find("BoneSource").transform.up = direction;

        line.SetPosition(1, direction * 7);
    }
    public void FireBone()
    {
        // disable line
        if (boneCdTimer <= 0 && bones > 0 && Time.timeScale == 1)
        {
            direction = joystickAim.ScaledValue;
            direction.Normalize();
            bonesThrownCount++;
            audioManager.Play("ThrowBone");
            GameObject boneInstance = Instantiate(bonePrefab, boneSourcePos.position, Quaternion.identity);
            Physics2D.IgnoreCollision(col, boneInstance.GetComponent<BoneControl>().GetCollider2D(), true);
            boneInstance.GetComponent<BoneControl>().initialVelocity(direction);
            boneCdTimer = boneCd;
            bones--;
        }
        else if (boneCdTimer <= 0 && bones == 0 && Time.timeScale == 1)
        {
            audioManager.Play("OutofBones");
            boneCdTimer = boneCd;
            hudControl.outOfBones();
        }
    }
}
