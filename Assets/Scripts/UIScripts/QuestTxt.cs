using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestTxt : MonoBehaviour
{
    public TextMeshProUGUI questTxt;
    public float fadeStartDelay; //time before the text fades in after start
    public float fadeIn; //duration of the text fading in

    public Color startColour;
   

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(TextFadeIn);
        startColour.a = 0;
        questTxt.color = startColour;
    }

    public IEnumerator TextFadeIn
    {
        get
        {
            yield return new WaitForSeconds(fadeStartDelay);

            float elapsedTime = 0;

            while (elapsedTime < fadeIn)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeIn);
                questTxt.color = new Color(startColour.r, startColour.g, startColour.b, alpha);
                yield return null;
            }

            questTxt.color = new Color(startColour.r, startColour.g, startColour.b, 1f);

        }
    }
}
