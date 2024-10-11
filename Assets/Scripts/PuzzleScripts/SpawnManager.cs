using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [Header("Stones Controls")]
    public List <GameObject> stoneToSpawn;

    [Header("Special Item Controls")]
    public GameObject healingGourd;
    public Transform gourdSpawn;

    public int matchCount = 0;
    public int mismatchCount = 0;
    public int maximumMismatchCount;
    public string SceneToLoad;

    private void Start()
    {
        foreach (GameObject stone in stoneToSpawn)
        {
            stone.SetActive(false);
        }
    }
    public void SpawnStone()
    {
        if (stoneToSpawn.Count != 0 )
        {
            stoneToSpawn[0].SetActive(true);

            matchCount++;
            stoneToSpawn.RemoveAt(0);
        }
        else
        {
            return;
        }
    }

    public void MismatchCube()
    {
        mismatchCount++;
        if (mismatchCount > maximumMismatchCount -1 )
        {
            SceneManager.LoadSceneAsync(SceneToLoad);
        }
    }

    public void SpawnGourd()
    {
            GameObject gourd = Instantiate(healingGourd, gourdSpawn.position, gourdSpawn.rotation);
            gourd.name = healingGourd.name;         
            return;
    }
}
