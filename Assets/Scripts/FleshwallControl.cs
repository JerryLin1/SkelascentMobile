using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshwallControl : MonoBehaviour
{
    Transform sprite;
    public float xShake = 0.005f;
    public float yShake = 0.005f;
    public float rbSpeed = 5f;
    public float acceleration = 0.5f;
    public float maxSpeed = 5f;
    private GameObject player;
    AudioManager audioManager;
    Rigidbody2D rb;
    bool stopped = false;
    float roarCd = 0f;
    void Start()
    {
        sprite = transform.Find("Sprite");
        rb = GetComponent<Rigidbody2D>();
        audioManager = GetComponent<AudioManager>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped == false && Time.timeScale == 1)
        {
            float x = Random.Range(-1f, 1f) * xShake;
            float y = Random.Range(-1f, 1f) * yShake;

            sprite.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

            rb.velocity = new Vector2(0, rbSpeed);
            if (rbSpeed <= maxSpeed)
            {
                rbSpeed += acceleration * Time.deltaTime;
            }
        }
        if (player.transform.position.y - transform.position.y <= -18)
        {
            stopped = true;
            rb.velocity = new Vector2(0, 0);
        }
        roarCd -= Time.deltaTime;
    }
    public void roar(float dist)
    {
        float vol;
        if (dist / 10 > 1) vol = 1;
        else vol = dist / 10;
        audioManager.sounds[0].source.volume = vol;
        audioManager.Play("Flesh");
        StartCoroutine(Camera.main.GetComponent<CameraControl>().cameraShake(0.25f, vol * 0.05f));
    }
    public void roarIfCan(float dist)
    {
        if (roarCd <= 0)
        {
            roar(dist);
            roarCd = Random.Range(10f, 20f);
        }
    }
    public void catchUp(float y)
    {
        if (transform.position.y < y)
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.transform.GetComponent<PlayerControl>().Die();
        }
    }
    public bool isStopped()
    {
        return stopped;
    }
}
