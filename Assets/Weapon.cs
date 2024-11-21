using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorruptSapSplash : MonoBehaviour
{
    public float appRate;
    public float duration;

    public AudioSource Splash;
    public AudioClip splashSFX;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.TryGetComponent(out IDamageable player))
        {
            StartCoroutine(UIManager.Instance.popUpControls.Splash(duration, appRate));
            Splash.PlayOneShot(splashSFX);
            player.DamageReceived(5);
        }

    }
}
