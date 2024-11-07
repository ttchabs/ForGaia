using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScroll : MonoBehaviour
{
    public float growSpeed = 50f; // Speed of growth in pixels per second
    public float maxHeight = 500f; // Maximum height of the text box

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Scroll();
    }


    public void Scroll()
    {
        if (rectTransform.sizeDelta.y < maxHeight)
        {
            // Increase the height over time
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + growSpeed * Time.deltaTime);
        }
    }
}
