using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
