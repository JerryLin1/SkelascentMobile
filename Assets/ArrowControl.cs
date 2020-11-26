using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2 position;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 9);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 11);
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(position, transform.position)> 7f) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PlayerControl>().Die();
            Destroy(gameObject);

        }

    }
}
