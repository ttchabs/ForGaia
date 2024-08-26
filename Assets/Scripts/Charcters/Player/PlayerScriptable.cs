using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Player/Player Container")]
public class PlayerScriptable : ScriptableObject
{
    public int maxPlayerHP = 5;

    public void LoseHP(FirstPersonControls playerHP, int damage)
    {
        playerHP.currentPlayerHP -= damage;
        if (playerHP.currentPlayerHP < 0)
        {
            PlayerDeath();
        }
    }

    public void GainHP(FirstPersonControls playerHP, int recover)
    {
        playerHP.currentPlayerHP += recover;
        if (playerHP.currentPlayerHP > maxPlayerHP)
            playerHP.currentPlayerHP = maxPlayerHP;
    }

    public void Knockback()
    {

    }
    public void PlayerDeath()
    {
        Debug.Log("Player is dead");
    }
}
