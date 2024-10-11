using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Player/Player Container")]
public class PlayerScriptable : ScriptableObject
{
    [Header("PLAYER IDENTIFICATION:")]
    [Space (5)]
    [SerializeField] string _playerName;
    [SerializeField] string _epithet;

    [Header("PLAYER STATISTICS:")]
    [Space (5)]
    [SerializeField] float _maxPlayerHP;
    [SerializeField] float _maxWeaponWeight;
    
    public float MaxPlayerHP { get => _maxPlayerHP; }
    public float MaxWeaponWeight { get => _maxWeaponWeight; }

    public void PlayerDeath()
    {
        Debug.Log("Player is dead");
    }
}
