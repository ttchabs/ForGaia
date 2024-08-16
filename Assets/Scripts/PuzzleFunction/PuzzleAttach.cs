using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PuzzleAttach : MonoBehaviour
{
    public Transform puzzleSlots;
    public FirstPersonControls playerSettings;

    private void Start()
    {
        //Find the FisrtPersonControls script in the project
        FirstPersonControls firstPersonControls = FindObjectOfType<FirstPersonControls>();

        //Place the script into the playerSettings field
        playerSettings = firstPersonControls;
    }

    //When a puzzle piece collides with the slots, it will be placed into the bin. if the colours correspond, it will add to a counter
    private void OnTriggerEnter(Collider other)
    {
        
        //Checks to see if an object with the specified tag is in contact with the trigger and is no longer being held by a player
        if (other.gameObject.CompareTag("PickUp") && playerSettings.heldObject.transform.parent == null)
        {
            Debug.Log("yes");

            other.transform.SetParent(puzzleSlots);
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;

            if (this.gameObject.name == "RedSlot" && other.gameObject.name == "RedPiece")
            {
                Debug.Log("Red");
            }

            if (this.gameObject.name == "BlackSlot" && other.gameObject.name == "BlackPiece")
            {
                Debug.Log("Black");
            }

            if (this.gameObject.name == "WhiteSlot" && other.gameObject.name == "WhitePiece")
            {
                Debug.Log("White");
            }

            if (this.gameObject.name == "GreySlot" && other.gameObject.name == "GreyPiece")
            {
                Debug.Log("Grey");
            }

            if (this.gameObject.name == "PurpleSlot" && other.gameObject.name == "PurplePiece")
            {
                Debug.Log("Purple");
            }

            if (this.gameObject.name == "YellowSlot" && other.gameObject.name == "YellowPiece")
            {
                Debug.Log("Yellow");
            }

            if (this.gameObject.name == "BlueSlot" && other.gameObject.name == "BluePiece")
            {
                Debug.Log("Blue");
            }

            if (this.gameObject.name == "GreenSlot" && other.gameObject.name == "GreenPiece")
            {
                Debug.Log("Green");
            }
        }
        
    }
}
