using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public bool spawnsEnemies;
    public bool spawnsPickups;
    public List<GameObject> possibleEnemySpawns;
    public List<GameObject> possiblePickupSpawns;
    List<GameObject> totalSpawns = new List<GameObject>();

    void Start()
    {
        totalSpawns.Add(new GameObject());
        if (spawnsEnemies)
        {
            totalSpawns.AddRange(possibleEnemySpawns);
        }
        if (spawnsPickups)
        {
            totalSpawns.AddRange(possiblePickupSpawns);
        }
        if (totalSpawns.Count != 0) {
            Instantiate(totalSpawns[Random.Range (0, totalSpawns.Count)], transform.position, Quaternion.identity);
        }
    }
}
