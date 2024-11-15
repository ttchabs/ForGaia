using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGain : MonoBehaviour
{
    public int healthRecovered;

    public PickUpScriptable HealthGrubsData;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddConsumableToInventory(HealthGrubsData);
            Destroy(gameObject);

        }
    }
}
