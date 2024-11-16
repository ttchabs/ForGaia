using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerPopUpEvents : MonoBehaviour
{

    public static PlayerPopUpEvents instance;
    [Header("DEATH EVENT POP UP:")]
    public CanvasGroup youDiedPopUp;

    public void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    public void ShowDeathEvent()
    {
        youDiedPopUp.gameObject.SetActive(true);
        StartCoroutine(DeathEventPopUp(5f));
    }
    public IEnumerator DeathEventPopUp(float duration) 
    {
        youDiedPopUp.alpha = 0;
        float timer = 0f;
        yield return null;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            youDiedPopUp.alpha = Mathf.Lerp(youDiedPopUp.alpha, 1, duration * Time.deltaTime);
            yield return null;
        }
        youDiedPopUp.alpha = 1; 
        yield return null;
    }
}
