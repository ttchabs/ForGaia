using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGain : PickUpFunction
{
    public int healthRecovered;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var ui = UIManager.Instance;
            ui.hudControls.PickUpGrub();
            Destroy(gameObject);  
        }
    }
}
