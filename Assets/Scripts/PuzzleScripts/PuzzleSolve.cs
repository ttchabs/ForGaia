using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleSolve : MonoBehaviour
{
    public Transform colorSlot;
    public SpawnManager spawnManager;
    public FirstPersonControls playerSettings;

    private Color _slotColor;
    private Color _pieceColour;

    private void Start()
    {
        colorSlot = this.GetComponent<Transform>();
        spawnManager = FindObjectOfType<SpawnManager>();
        playerSettings = FindObjectOfType<FirstPersonControls>();

        Renderer slotRenderer = GetComponentInChildren<Renderer>();
        _slotColor = slotRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        Renderer _pieceRenderer = other.GetComponent<Renderer>();
        _pieceColour = _pieceRenderer.material.color;

        if (other.gameObject.CompareTag("SorterPuzzleStone") && playerSettings.heldObject.transform.parent == null)
        {
            Collider _collider = this.gameObject.GetComponent<Collider>();

            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.SetParent(colorSlot);
            other.transform.localPosition = new Vector3(0, 0.1f, 0);
            other.transform.localRotation = Quaternion.identity;

            if (_pieceColour == _slotColor)
            {
                spawnManager.SpawnStone();
                _collider.enabled = false;

                if (_pieceColour == Color.white && _slotColor == Color.white) 
                {
                    spawnManager.SpawnGourd();
                    _collider.enabled = false;
                }
            }
            else
            {
                return;
            }
        }
    }
}


