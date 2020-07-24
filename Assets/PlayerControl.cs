using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float movementSpeed = 3f;
    float jumpSpeed = 500f;
    bool isGrounded = true;

    function Update()
    {
        if (isGrounded)
        {
            rigidbody2D.gravityScale = 2.1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            transform.rotation.y = 180; //This is for rotating sprite images
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            transform.rotation.y = 0; //This is for rotating sprite images
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!isGrounded)
            {
                rigidbody2D.gravityScale += 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded)
            {
                rigidbody2D.AddForce(Vector3.up * jumpSpeed);
                isGrounded = false;
            }
        }
    }

    function OnCollisionEnter2D(col Collision2D)
    {
        isGrounded = true;
    }
}
