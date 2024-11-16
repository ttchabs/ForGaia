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
    [SerializeField] AudioClip _deathSFX;
    [SerializeField] AudioClip _playerHitSFX;

    [Header("PLAYER STATISTICS:")]
    [Space (5)]
    [SerializeField] int _maxPlayerHP;
    [SerializeField] float _maxWeaponWeight;
    
    public string PlayerName => _playerName;
    public string Epithet => _epithet;
    public AudioClip WalkSFX => _walkSFX;
    public int MaxPlayerHP => _maxPlayerHP;
    public float MaxWeaponWeight => _maxWeaponWeight; 

    public IEnumerator PlayerDeath(string sceneToLoad)
    {

        yield return new WaitForSeconds(7f);
        GameManager.managerInstance.ReloadGame(sceneToLoad);
    }
}
