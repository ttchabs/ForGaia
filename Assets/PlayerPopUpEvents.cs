using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerPopUpEvents : MonoBehaviour
{
    //public static PlayerPopUpEvents instance;
    [Header("DEATH EVENT POP UP:")]
    public CanvasGroup youDiedPopUp;

    [Header("SLUDGE POPUP:")]
    public CanvasGroup splashImage;

    [Header("ITEM PICKED UP POPUP")]
    public CanvasGroup itemPickUpAlpha;
    public TextMeshProUGUI itemPickUpText;
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

    public IEnumerator Splash(float duration, float appearanceRate)
    {
        splashImage.gameObject.SetActive(true);
        splashImage.alpha = 0;
        float timer = 0f;
        yield return null;

        while (timer < appearanceRate)
        {
            timer += Time.deltaTime;
            splashImage.alpha = Mathf.Lerp(splashImage.alpha, 1, appearanceRate * Time.deltaTime);
            yield return null;
        }
        splashImage.alpha = 1;
        yield return null;

        yield return new WaitForSeconds(duration);
        float disappear = 0;
        while(disappear < 5)
        {
            disappear += Time.deltaTime;
            splashImage.alpha = Mathf.Lerp(splashImage.alpha, 0, 5 * Time.deltaTime);
            yield return null;
        }
        splashImage.alpha = 0;
        splashImage.gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator ItemToInventoryNotfication(string name, float timeVisible)
    {
        itemPickUpAlpha.gameObject.SetActive(true);
        itemPickUpAlpha.alpha = 0;
        itemPickUpText.text = $"{name} (Press I)";

        float timer = 0;
        yield return null;
        while(timer < timeVisible)
        {
            timer += Time.deltaTime;
            itemPickUpAlpha.alpha = Mathf.Lerp(itemPickUpAlpha.alpha, 1, timeVisible * Time.deltaTime);
            yield return null;
        }
        itemPickUpAlpha.alpha = 0;
        itemPickUpAlpha.gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator ItemToHandNotification(string name, float timeVisible)
    {
        itemPickUpAlpha.gameObject.SetActive(true);
        itemPickUpAlpha.alpha = 0;
        itemPickUpText.text = $"{name} in hand";

        float timer = 0;
        float fadeOut = 0;
        yield return null;
        while (timer < timeVisible)
        {
            timer += Time.deltaTime;
            itemPickUpAlpha.alpha = Mathf.Lerp(itemPickUpAlpha.alpha, 1, timeVisible * Time.deltaTime);
            yield return null;
        }

        while (fadeOut < timeVisible)
        {
            fadeOut += Time.deltaTime;
            itemPickUpAlpha.alpha = Mathf.Lerp(itemPickUpAlpha.alpha, 0, timeVisible * Time.deltaTime);
            yield return null;
        }
        itemPickUpAlpha.alpha = 0;
        itemPickUpAlpha.gameObject.SetActive(false);
        yield return null;
    }
}
