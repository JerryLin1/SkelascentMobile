using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // if too far away, shrink and dissapate
        if (Vector2.Distance(position, transform.position) > 7f)
        {
            transform.DOScale(Vector3.zero, 1);
            if (transform.localScale.x <= 0.1f) Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PlayerControl>().Die();

        }
        Destroy(gameObject);
    }
}
