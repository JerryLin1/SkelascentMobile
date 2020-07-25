using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnNextRoom : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform player;
    private float roomSize;
    private int difficulty;
    private RoomTemplates templates;
    

    void Start()
    {
        Invoke("Spawn", 0.5f);
        // float nextRoomSize = newRoom.transform.Find("Walls").GetComponent<TilemapRenderer>().bounds.size.y / 2;
        // newRoom.transform.position = new Vector3(newRoom.transform.position.x, newRoom.transform.position.y + nextRoomSize, newRoom.transform.position.z);
        
    }

    void Spawn() {
        roomSize = transform.Find("Walls").GetComponent<TilemapRenderer>().bounds.size.y / 2;
        templates = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>();
        player = GameObject.Find("Player").transform;

        // eventually set difficulty to some value based off score
        // 0 easy, 1 medium, 2 hard
        difficulty = 0;
        GameObject[] rooms;

        if (difficulty == 0) {
            rooms = templates.easyRooms;
        } else if (difficulty == 1) {
            rooms = templates.mediumRooms;
        } else {
            rooms = templates.hardRooms;
        }

        GameObject newRoom;
        int rand = Random.Range(0, rooms.Length-1);
        newRoom = Instantiate(rooms[rand], transform.position, Quaternion.identity);
        newRoom.transform.SetParent(GameObject.Find("Grid").transform);
        
    }

    void Update()
    {
        
        if (player.position.y > transform.position.y + roomSize) {
            // Move camera and go to next room

            // Add points
        }

    }
}
