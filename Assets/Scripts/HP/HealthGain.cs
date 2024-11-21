using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGain : MonoBehaviour
{
    public int healthRecovered;

    public ItemScriptable HealthGrubsData;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var ui = UIManager.Instance;
            ui.invetoryControls.AddConsumableToInventory(HealthGrubsData);
            ui.hudControls.PickUpGrub();
            Destroy(gameObject);
            
        }
    }
}
