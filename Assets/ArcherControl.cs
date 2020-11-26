using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherControl : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject deathParticlePrefab;
    Rigidbody2D rb;
    Animator animator;
    Transform player;
    AudioManager audioManager;
    float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.position, transform.position) < 6f && cooldown <= 0f) {
            transform.eulerAngles = (player.position.x > transform.position.x) ? new Vector3(0, 180, 0) : new Vector3(0,0,0);

            animator.SetTrigger("attacking");
            cooldown = 1.75f;
        }
        cooldown-=Time.deltaTime;
    }

    public void FireArrow() {
        Vector2 direction = (player.position-transform.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.transform.right = -direction;
        arrow.GetComponent<Rigidbody2D>().AddForce(direction*300f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.gameObject.tag == "Bone") {
            GameObject.Find("Player").GetComponent<PlayerControl>().demonKilled(transform);
            die();
        }
    }
    public void die () {  
        Instantiate (deathParticlePrefab, transform.position, Quaternion.identity);      
        Destroy(gameObject);
    }

}
