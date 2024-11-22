using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestOpenScript : MonoBehaviour , IDamageable
{
    public event IDamageable.DamageReceivedEvent OnDamageReceived;

    public Animator openAnimation;
    public AudioSource chestHitSFX;

    public AudioClip chestHitAudio;
    public AudioClip chestOpenSound;

    public Collider chestCol;

    //public Canvas chestOpenCanvas;
    public void DamageReceived(int damageAmount)
    {
        StartCoroutine(OpenChest());
    }
    
    public IEnumerator OpenChest()
    {
        chestCol.enabled = false;
        chestHitSFX.PlayOneShot(chestHitAudio);
        yield return new WaitForSeconds(1.5f);
        openAnimation.SetTrigger("openChest");
        chestHitSFX.PlayOneShot(chestOpenSound);
    }
}
