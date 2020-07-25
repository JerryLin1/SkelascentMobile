using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnNextRoom : MonoBehaviour
{
    // Start is called before the first frame update

    public bool spawnNextRoom = false;

    private float roomSize;
    private bool spawnedNextRoom = false;
    private Transform player;
    private int difficulty;
    private RoomTemplates templates;

    private Vector3 newRoomOffset;
    public bool leavingRoom = false;
    private bool leftRoom = false;

    private GameObject newRoom;

    void Spawn() {
        roomSize = transform.Find("Backdrop").GetComponent<TilemapRenderer>().bounds.size.y / 2;
        templates = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>();
        player = GameObject.Find("Player").transform;

        // eventually set difficulty to some value based off score
        // 0 easy, 1 medium, 2 hard
        difficulty = 0;
        GameObject[] rooms;

        // Get room from list depending on difficulty
        if (difficulty == 0) {
            rooms = templates.easyRooms;
            newRoomOffset = new Vector3(0, templates.easySize, 0);
        } else if (difficulty == 1) {
            rooms = templates.mediumRooms;
            newRoomOffset = new Vector3(0, templates.mediumSize, 0);
        } else {
            rooms = templates.hardRooms;
            newRoomOffset = new Vector3(0, templates.hardSize, 0);
        }

        // Spawn room
        int rand = Random.Range(0, rooms.Length);
        newRoom = Instantiate(rooms[rand], transform.position + new Vector3(0,roomSize,0) + newRoomOffset, Quaternion.identity);
        newRoom.transform.SetParent(GameObject.Find("Grid").transform);
        
        
    }

    void Update()
    {
        // Spawn new room above when entering a room
        if (spawnNextRoom && !spawnedNextRoom) {
            Spawn();
            spawnedNextRoom = true;
        }

        // Move camera to next room, spawn player on entry point, and delete old room
        if (leavingRoom && !leftRoom) {
            GameObject camera = GameObject.Find("Main Camera");
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + newRoomOffset.y + roomSize, camera.transform.position.z);
            
            player.position = newRoom.transform.Find("Platforms/EntryPlatform/Point").transform.position + new Vector3(0,1,0);
            leftRoom = true;
            Destroy(gameObject);
        }

        
        

    }
}
