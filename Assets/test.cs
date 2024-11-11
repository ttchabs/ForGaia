using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public GameObject thisItem;

    public void TurnOff(){
        thisItem.SetActive(false);
    }
}
