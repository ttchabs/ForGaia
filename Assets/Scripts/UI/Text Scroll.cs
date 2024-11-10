using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextScroll : MonoBehaviour
{
    public float growSpeed = 50f; // Speed of growth in pixels per second
    public float maxHeight = 500f; // Maximum height of the text box
    public float scrollTime;

    public Animator fadeToBlack;

    public RectTransform rectTransform;

    void Start()
    {
        //rectTransform = GetComponent<RectTransform>();

    }

    void Update()
    {
        Scroll();
        StartCoroutine(ScrollIng);
        Invoke("PlayAnim", scrollTime - 5f);
    }


    public void Scroll()
    {
        if (rectTransform.sizeDelta.y < maxHeight)
        {
            // Increase the height over time
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + growSpeed * Time.deltaTime);
        }
    }


    public IEnumerator ScrollIng
    {
        get
        {


            yield return new WaitForSeconds(scrollTime);
            LoadScene("MainScene");

        }
    }
    public void PlayAnim()
    {
        fadeToBlack.SetTrigger("StartFade");
    }

    public void LoadScene(string MainScene)
    {
        SceneManager.LoadScene(MainScene);

    }

    
}
