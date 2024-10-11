using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleSolve : MonoBehaviour
{
    public SpawnManager spawnManager;

    private Material _slotColor;
    private Material _pieceColour;

    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();

        Material slotRenderer = GetComponent<Material>();
        _slotColor = slotRenderer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SorterPuzzleStone") && other.gameObject.GetComponent<Rigidbody>().isKinematic == false )
        {          
            Material _pieceRenderer = other.GetComponent<Material>();
            _pieceColour = _pieceRenderer;

            Collider _collider = gameObject.GetComponent<Collider>();

            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0, 0.12f, 0);
            other.transform.localRotation = Quaternion.identity;

            if (_pieceColour == _slotColor)
            {
                spawnManager.SpawnStone();
                _collider.enabled = false;

                if (spawnManager.matchCount > 6) 
                {
                    spawnManager.SpawnGourd();
                    _collider.enabled = false;
                }
            }
            else if (_pieceColour != _slotColor)
            {
                spawnManager.MismatchCube();
                Debug.Log("WrongCube");
            }
        }
    }
}


