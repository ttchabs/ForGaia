using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleSolve : MonoBehaviour
{
    public SpawnManager spawnManager;

    public Material _slotColor;
    public Material _pieceColour;

    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();

        Renderer slotRenderer = GetComponent<Renderer>();
        _slotColor = slotRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SorterPuzzleStone") && other.gameObject.GetComponent<Rigidbody>().isKinematic == false )
        {          
            Renderer _pieceRenderer = other.GetComponentInChildren<Renderer>();
            _pieceColour = _pieceRenderer.material;

            Collider _collider = gameObject.GetComponent<Collider>();

            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0, 0.12f, 0);

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
            else
            {
                spawnManager.MismatchCube();
                Debug.Log("WrongCube");
            }
        }
    }
}


