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
        gong.Stop();
    }
}
