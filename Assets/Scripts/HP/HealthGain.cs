using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGain : MonoBehaviour
{
    public int healthRecovered;
    public FirstPersonControls playerHealth;

    public PickUpScriptable HealthGrubsData;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddConsumable(HealthGrubsData);
            Destroy(gameObject);
        }
    }
}
