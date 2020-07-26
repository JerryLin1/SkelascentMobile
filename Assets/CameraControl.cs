using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public GameObject fleshWall;
    float playerFleshDist;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fleshWall.GetComponent<FleshwallControl>().isStopped() == false)
        {
            Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            // playerFleshDist = player.transform.position.y - fleshWall.transform.position.y;
            // float magnitude;
            // if (playerFleshDist >= 1) magnitude = 1 / playerFleshDist;
            // else magnitude = 1;
            // float x = Random.Range(-1f, 1f) * magnitude;
            // float y = Random.Range(-1f, 1f) * magnitude;

            // transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        }
    }
    public IEnumerator cameraShake(float duration, float magnitude)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}