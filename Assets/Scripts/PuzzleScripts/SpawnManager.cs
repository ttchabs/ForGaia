using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Stones Controls")]
    public List <GameObject> stoneToSpawn;

    [Header("Spawn Controls")]
    public List <Transform> spawnPoints;

    [Header("Special Item Controls")]
    public GameObject healingGourd;
    public Transform gourdSpawn;

    public int matchCount;

    public void SpawnStone()
    {


        if (stoneToSpawn.Count != 0 && spawnPoints.Count != 0)
        {
            GameObject stone = Instantiate(stoneToSpawn[0], spawnPoints[0].position, spawnPoints[0].rotation);
            stone.name = stoneToSpawn[0].name;
            matchCount++;
            
            stoneToSpawn.RemoveAt(0);
            spawnPoints.RemoveAt(0);
        }
        else
        {
            return;
        }

    }

    public void SpawnGourd()
    {
        if (matchCount >= 7)
        {
            GameObject gourd = Instantiate(healingGourd, gourdSpawn.position, gourdSpawn.rotation);
            gourd.name = healingGourd.name;
            Debug.Log("Works");
            return;
        }
    }
}
