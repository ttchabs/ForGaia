using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationNameTxt : MonoBehaviour
{
    public TextMeshProUGUI questTxt;
    public float fadeStartDelay; //time before the text fades in after start
    public float fadeOutDelay;
    public float fadeIn; //duration of the text fading in
    public float fadeOut;
    public float moveUp;


    
   // private Transform text;


    public Color startColour;


    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(TextFadeIn);
        startColour.a = 0;
        questTxt.color = startColour;
        //startPosition = questTxt.transfrom.position;

        
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


               // float moveAmount = Mathf.Lerp(0,MoveUp,elapsedTime/fadeIn);
               // text.transfrom.position += new Vector3(0, moveUp, 0);
                yield return null;
            }

            questTxt.color = new Color(startColour.r, startColour.g, startColour.b, 1f);
           // text.transfrom.position += new Vector3(0, moveUp, 0);
            StartCoroutine (TextFadeOut);

        }
    }
    public IEnumerator TextFadeOut
    {
        get
        {
            yield return new WaitForSeconds(fadeOutDelay);

            float elapsedTime = 0;

            while (elapsedTime < fadeIn)
            {
                elapsedTime += Time.deltaTime;
                float alpha = 1.0f - Mathf.Clamp01(elapsedTime / fadeOut);
                questTxt.color = new Color(startColour.r, startColour.g, startColour.b, alpha);
                yield return null;
            }

            questTxt.color = new Color(startColour.r, startColour.g, startColour.b, 0f);

        }
    }
}
