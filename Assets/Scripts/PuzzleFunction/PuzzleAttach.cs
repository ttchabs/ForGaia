using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAttach : MonoBehaviour
{
    public Transform puzzleSlots;
    private GameObject _puzzlePieceIn;
    

    private void OnTriggerStay(Collider other)
    {

        _puzzlePieceIn = GameObject.Find("item");


        if (other.gameObject == _puzzlePieceIn) 
        {
            Debug.Log("yest");

            //_puzzlePieceIn.transform.SetParent(puzzleSlots);
            _puzzlePieceIn.GetComponent<Rigidbody>().isKinematic = true;
            _puzzlePieceIn.transform.position = puzzleSlots.position;
            _puzzlePieceIn.transform.rotation = puzzleSlots.rotation;
            _puzzlePieceIn.transform.parent = puzzleSlots;
        }
    }
}
