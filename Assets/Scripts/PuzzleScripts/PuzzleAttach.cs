using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAttach : MonoBehaviour
{
    [Header("Slots Controls")]
    public Transform puzzleSlots;
    public FirstPersonControls playerSettings;
    //private GameObject _placedStone;

    public StoneManager stoneManager;


    private void Start()
    {
        //Find the FisrtPersonControls script in the project
        FirstPersonControls firstPersonControls = FindObjectOfType<FirstPersonControls>();
        //Place the script into the playerSettings field
        playerSettings = firstPersonControls;

        StoneManager stones = FindObjectOfType<StoneManager>();
        stoneManager = stones;
    }

    //When a puzzle piece collides with the slots, it will be placed into the bin. if the colours correspond, it will add to a counter
    private void OnTriggerEnter(Collider other)
    {
        //Checks to see if an object with the specified tag is in contact with the trigger and is no longer being held by a player
        if (other.gameObject.CompareTag("SorterPuzzleStone") && playerSettings.heldObject.transform.parent == null)
        {
            Collider _collider = this.gameObject.GetComponent<Collider>();

            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.SetParent(puzzleSlots);
            other.transform.localPosition = new Vector3(0, 0.1f, 0);
            other.transform.localRotation = Quaternion.identity;

            //when the stone colour matches the tile colour, it will spawn another stone and deactivate the trigger on the tile.
            #region COLOUR MATCHER:
            if (this.gameObject.name == "RedTile" && other.gameObject.name == "RedStone")
            {
                stoneManager.SpawnOrange();
                _collider.enabled = false;
                Debug.Log("Red");
            }

            if (this.gameObject.name == "OrangeTile" && other.gameObject.name == "OrangeStone(Clone)")
            {
                stoneManager.SpawnYellow();
                _collider.enabled = false;
                Debug.Log("Yellow");
            }

            if (this.gameObject.name == "YellowTile" && other.gameObject.name == "YellowStone(Clone)")
            {
                stoneManager.SpawnGreen();
                _collider.enabled = false;
                Debug.Log("Yellow");
            }

            if (this.gameObject.name == "GreenTile" && other.gameObject.name == "GreenStone(Clone)")
            {
                stoneManager.SpawnBlue();
                _collider.enabled = false;
                Debug.Log("Green");
            }

            if (this.gameObject.name == "BlueTile" && other.gameObject.name == "BlueStone(Clone)")
            {
                stoneManager.SpawnPurple();
                _collider.enabled = false;
                Debug.Log("Blue");
            }

            if (this.gameObject.name == "PurpleTile" && other.gameObject.name == "PurpleStone(Clone)")
            {
                stoneManager.SpawnBlack();
                _collider.enabled = false;
                Debug.Log("Purple");
            }

            if (this.gameObject.name == "BlackTile" && other.gameObject.name == "BlackStone(Clone)")
            {
                stoneManager.SpawnGrey();
                _collider.enabled = false;
                Debug.Log("Black");
            }

            if (this.gameObject.name == "GreyTile" && other.gameObject.name == "GreyStone(Clone)")
            {
                stoneManager.SpawnWhite();
                _collider.enabled = false;
                Debug.Log("Grey");
            }

            if (this.gameObject.name == "WhiteTile" && other.gameObject.name == "WhiteStone(Clone)")
            {
                stoneManager.SpawnGourd();
                _collider.enabled = false;
                Debug.Log("White");
            }
            #endregion
        }

    }

    
}

