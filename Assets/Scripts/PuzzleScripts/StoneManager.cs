using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    //public GameObject[] stonesInPlace;

    [Header("Stones Controls")]
    public GameObject redStone;
    public GameObject orangeStone;
    public GameObject yellowStone;
    public GameObject greenStone;
    public GameObject blueStone;
    public GameObject purpleStone;
    public GameObject blackStone;
    public GameObject greyStone;
    public GameObject whiteStone;



    [Header("Stone Spawn Controls")]
    public Transform orangeSpawn;
    public Transform yellowSpawn;
    public Transform greenSpawn;
    public Transform blueSpawn;
    public Transform purpleSpawn;
    public Transform blackSpawn;
    public Transform greySpawn;
    public Transform whiteSpawn;

    [Header("Special Item Controls")]
    public GameObject healingGourd;
    public Transform gourdSpawn;

    //when a stone is placed, a new stone will be instantiated in the same room
    #region INSTANTIATION METHODS:
    public void SpawnOrange()
    {
        GameObject go1 = new GameObject(orangeStone.name);
        Instantiate(go1, orangeSpawn.position, orangeSpawn.rotation);
    }

    public void SpawnYellow()
    {
        Instantiate(yellowStone, yellowSpawn.position, yellowSpawn.rotation);
    }

    public void SpawnGreen()
    {
        Instantiate(greenStone, greenSpawn.position, greenSpawn.rotation);
    }

    public void SpawnBlue()
    {
        Instantiate(blueStone, blueSpawn.position, blueSpawn.rotation);
    }

    public void SpawnPurple()
    {
        Instantiate(purpleStone, purpleSpawn.position, purpleSpawn.rotation);
    }

    public void SpawnBlack()
    {
        Instantiate(blackStone, blackSpawn.position, blackSpawn.rotation);
    }

    public void SpawnGrey()
    {
        Instantiate(greyStone, greySpawn.position, greySpawn.rotation);
    }

    public void SpawnWhite()
    {
        Instantiate(whiteStone, whiteSpawn.position, whiteSpawn.rotation);
    }

    public void SpawnGourd()
    {
        Instantiate(healingGourd, gourdSpawn.position, gourdSpawn.rotation);
    }
    #endregion
}
