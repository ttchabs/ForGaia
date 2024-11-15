using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewItem", menuName = "Player/Player Container")]
public class PlayerScriptable : ScriptableObject
{
    [Header("PLAYER IDENTIFICATION:")]
    [Space (5)]
    [SerializeField] string _playerName;
    [SerializeField] string _epithet;
    [SerializeField] AudioClip _walkSFX;

    [Header("PLAYER STATISTICS:")]
    [Space (5)]
    [SerializeField] int _maxPlayerHP;
    [SerializeField] float _maxWeaponWeight;
    
    public string PlayerName => _playerName;
    public string Epithet => _epithet;
    public AudioClip WalkSFX => _walkSFX;
    public int MaxPlayerHP => _maxPlayerHP;
    public float MaxWeaponWeight => _maxWeaponWeight; 

    public void PlayerDeath(string sceneToLoad)
    {
        GameManager.managerInstance.ReloadGame(sceneToLoad);
        Debug.Log("Player is dead");
    }
}
