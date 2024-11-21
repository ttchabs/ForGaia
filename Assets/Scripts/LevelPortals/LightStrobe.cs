using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStrobe : MonoBehaviour
{

    public Light portalLight;
    public float strobeRate;
    public float strobeMax;
    public float strobeMin;

    public float currentTime;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (portalLight != null)
        {
            currentTime = Time.time * strobeRate;

            float strobe = Mathf.Lerp(strobeMin, strobeMax, (Mathf.Sin(currentTime) + 1f) / 2f);

            portalLight.intensity = strobe;
        }
    }
}
