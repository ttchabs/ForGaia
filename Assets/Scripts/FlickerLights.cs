using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLights : MonoBehaviour
{
    public Light flickerLight;
    public float minWait;
    public float maxWait;

    private void Awake()
    {
        StartCoroutine(LightFLicker());
    }

    public IEnumerator LightFLicker()
    {
        while (true)
        {
            yield return new WaitForSeconds (Random.Range(minWait, maxWait));
            flickerLight.enabled = ! flickerLight.enabled;
        }
    }
}
