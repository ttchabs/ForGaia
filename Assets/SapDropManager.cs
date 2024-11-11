using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapDropManager : MonoBehaviour
{
    public GameObject sap;
    public List<Transform> spawnpos;

    public float timeToDespawn;
    public void Start()
    {
        InvokeRepeating("ChooseRandomSpawn", 2f, 2f);
    }

    public void ChooseRandomSpawn()
    {
        int randomPick1 = Random.Range(0, spawnpos.Count);
        Transform spawn1 = spawnpos[randomPick1];
        GameObject newSap1 = Instantiate(sap, spawn1);
        newSap1.transform.position = spawn1.transform.position;
        Destroy(newSap1, timeToDespawn);
        return;
    }
}
