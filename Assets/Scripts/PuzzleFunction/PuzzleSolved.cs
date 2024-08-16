using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolved : MonoBehaviour
{
    public Transform colorSlot;
    private Color _pieceColour;
    private Color _slotColor;

    private void Start()
    {
        Renderer slotRenderer = GetComponentInChildren<Renderer>();
        _slotColor = slotRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        Renderer _pieceRenderer = other.GetComponent<Renderer>();
        _pieceColour = _pieceRenderer.material.color;

        if (_pieceColour == _slotColor) 
        {
            Debug.Log("Match");
        }
        else
        {
            Debug.Log("Nope");
        }

    }
}
