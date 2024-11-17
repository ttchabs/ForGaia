using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevels : MonoBehaviour
{
    public string toWhichLevel;

    //public Animator portalTransition;

    public float switchSceneInterval;


    public void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(toWhichLevel);
    }

/*    public IEnumerator NextScene(float timer)
    {
        portalTransition.SetTrigger("StartFade");
        portalTransition.gameObject.SetActive(true);
        yield return null;
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(toWhichLevel);
    }*/


}
