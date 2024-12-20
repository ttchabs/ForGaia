using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewItem", menuName = "Characters/Player/Player Data Container")]
public class PlayerScriptable : ScriptableObject
{
    [Header("PLAYER IDENTIFICATION:")]
    [Space(5)]
    [SerializeField] string _playerName;
    [SerializeField] [TextArea] string _epithet;

    [Header("PLAYER SFX:")]
    [Range(0f, 1f)] [SerializeField] float _volume;
    [SerializeField] AudioClip _walkSFX;
    [SerializeField] AudioClip _playerHitSFX;

    [Header("PLAYER STATISTICS:")]
    [Space (5)]
    [SerializeField] int _maxPlayerHP;
    [SerializeField] float _maxWeaponWeight;
    
    public string PlayerName => _playerName;
    public string Epithet => _epithet;
    public float Volume => _volume;
    public AudioClip WalkSFX => _walkSFX;
    public AudioClip PlayerHitSFX => _playerHitSFX;
    public int MaxPlayerHP => _maxPlayerHP;
    public float MaxWeaponWeight => _maxWeaponWeight; 

    public IEnumerator PlayerDeath(string sceneToLoad, Animator deathAnimation)
    {
        UIManager.Instance.popUpControls.ShowDeathEvent();
        deathAnimation.SetBool("isDead", true);
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(sceneToLoad);
    }



    public void PlayPlayerHitSFX(AudioSource playerHit)
    {
        playerHit.PlayOneShot(PlayerHitSFX, Volume);
    }
}
