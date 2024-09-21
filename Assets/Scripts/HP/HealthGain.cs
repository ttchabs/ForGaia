using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGain : MonoBehaviour
{
    public int healthRecovered;
    public FirstPersonControls playerHealth;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            IDamageable recover = other.GetComponent<IDamageable>();
            recover.DamageReceived(-healthRecovered);
            if (playerHealth.currentPlayerHP >= playerHealth.playerConfigs.maxPlayerHP)
                playerHealth.currentPlayerHP = playerHealth.playerConfigs.maxPlayerHP;

            print("success");
            Destroy(gameObject);


        }
    }
}
