using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public List<GameObject> gameObjects;

    public void Tester(GameObject gameObject)
    {
        gameObjects.FirstOrDefault();
    }
}
