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

    void Start() {
        roomSize = transform.Find("Backdrop").GetComponent<TilemapRenderer>().bounds.size.y / 2;
        player = GameObject.Find("Player").transform;

    }
    void Spawn() {
        templates = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>();

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
        Debug.Log(roomSize + " " + newRoomOffset.y);
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
        if (player.position.y > transform.position.y + roomSize && !leftRoom) {         
            templates.offScreenRooms.Add(gameObject);
            leftRoom = true;
            if (templates.offScreenRooms.Count > 2) {
                GameObject roomToDelete = templates.offScreenRooms[0];
                templates.offScreenRooms.RemoveAt(0);
                foreach (Transform spawner in roomToDelete.transform.Find("Spawners").transform) {
                    Destroy(spawner.GetComponent<SpawnerControl>().entity);
                }
                GameObject.Find("FleshWall").GetComponent<FleshwallControl>().catchUp(roomToDelete.transform.position.y);
                Destroy(roomToDelete);
            }
            
        }

        
        

    }
}
