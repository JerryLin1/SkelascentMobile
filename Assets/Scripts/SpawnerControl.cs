using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public bool spawnsEnemies;
    public bool spawnsPickups;
    public List<GameObject> possibleEnemySpawns;
    public List<GameObject> possiblePickupSpawns;
    public GameObject entity = null;
    List<GameObject> totalSpawns = new List<GameObject>();
    

    void Start()
    {
        if (spawnsEnemies)
        {
            totalSpawns.AddRange(possibleEnemySpawns);
        }
        if (spawnsPickups)
        {
            totalSpawns.AddRange(possiblePickupSpawns);
        }
        if (totalSpawns.Count != 0) {
            int rand = Random.Range (0, totalSpawns.Count + 2);
            if (rand != totalSpawns.Count + 1) {
                entity = Instantiate(totalSpawns[Random.Range (0, totalSpawns.Count)], transform.position, Quaternion.identity);

            } 
        }
    }
}
