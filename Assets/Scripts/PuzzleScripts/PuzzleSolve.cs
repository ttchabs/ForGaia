using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleSolve : MonoBehaviour
{
    public SpawnManager spawnManager;

    public int stoneID;
    public int tileNumber;

    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SorterPuzzleStone") && other.gameObject.GetComponent<Rigidbody>().isKinematic == false )
        {          

            Collider _collider = gameObject.GetComponent<Collider>();
            StoneIdentity idNumber = other.GetComponent<StoneIdentity>();
            stoneID = idNumber.stoneNumber;

            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0, 0.12f, 0);

            if (tileNumber == stoneID)
            {
                spawnManager.SpawnStone();
                _collider.enabled = false;

                if (spawnManager.matchCount > 6) 
                {
                    spawnManager.SpawnGourd();
                    _collider.enabled = false;
                }
            }
            else
            {
                spawnManager.MismatchCube();
            }
        }
    }
}


