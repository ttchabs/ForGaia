using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevels : MonoBehaviour
{
    public string toWhichLevel;

    public Animator portalTransition;

    public float switchSceneInterval;

    public void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(NextScene(switchSceneInterval));
    }

    public IEnumerator NextScene(float timer)
    {
        portalTransition.SetTrigger("StartFade");
        yield return null;
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(toWhichLevel);
    }


}
