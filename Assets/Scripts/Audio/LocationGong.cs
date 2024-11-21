using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationGong : MonoBehaviour
{

    public AudioSource gong;

    public float gongLength;


    // Start is called before the first frame update
    void Start()
    {
        gong.Play();
        StartCoroutine(StopGong(gongLength));
    }

  IEnumerator StopGong(float gongLength)
    {

        yield return new WaitForSeconds(gongLength);

        float startVolume= gong.volume;
        for (float t = 0; t < gongLength; t += Time.deltaTime)
        {
            gong.volume = Mathf.Lerp(startVolume, 0, t / gongLength);
            yield return null;
        }
        gong.volume= 0;

            //gong.Stop();
    }
}
