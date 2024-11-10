using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("Level 1");
        //UIManger.instance.ChangeScenes();
    }
}
