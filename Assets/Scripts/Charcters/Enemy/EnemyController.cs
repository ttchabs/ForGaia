using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable
{

    [Header("STATS CONTROLS:")]
    public EnemyScriptable enemyConfigs; //data container for enemy stats
    public int enemyCurrentHP;

    [Header("ENEMY TRANSFORMS:")]
    public Transform player;
    private Rigidbody rb;

    [Header("ENEMY HEALTH DISPLAY")]
    public Slider enemyHealth;
    Transform camToFace;

    [Header("ENEMY MODEL AND ANIMATIONS:")]
    public Animator enemyAnimations;
    public AudioSource enemySFX;
    public AudioClip enemySFXClip;
    [SerializeField] float stepInterval = 18f;
    float stepTime = 0;

    public event IDamageable.DamageReceivedEvent OnDamageReceived;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camToFace = player.GetComponentInChildren<Camera>().transform;
        enemyAnimations = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        player = FirstPersonControls.Instance.transform;

        SetMaxEnemyHP();        
    }

    void Update()
    {
        if(FirstPersonControls.Instance.currentPlayerHP > 0) 
        {
            TrackPlayer();
        }
    }

    void LateUpdate()
    {
        enemyHealth.transform.LookAt(camToFace.position + camToFace.forward);
    }
    
    public void PlaySounds()
    {
        stepTime += Time.deltaTime;
        if(stepTime > stepInterval)
        {
            enemySFX.PlayOneShot(enemySFXClip);
            stepTime = 0;
        }
    }
    public void TrackPlayer()
    {
        float distanceBetween = Vector3.Distance(transform.position, player.position); //checks the distance between the player and the enemy, stores the value

        if (distanceBetween < 20f && distanceBetween > 2f && enemyCurrentHP > 0)
        {
            enemyAnimations.SetBool("isWalking", true);
            HandleEnemyTracking();

        }
        else
        {
            if(enemyAnimations != null)
                enemyAnimations.SetBool("isWalking", false);
   
        }
        
        if (distanceBetween < 1f && enemyCurrentHP > 0)
        {
            enemyAnimations.SetBool("isWalking",false);
        }
    }

    public void HandleEnemyTracking()
    {
        Vector3 direction = transform.position - player.position; //checks the coords of the player while ignoring the y-axis
        direction.y = 0;
        Quaternion lookDirection = Quaternion.LookRotation(direction); //makes the enemy face the player
        transform.rotation = lookDirection;
        lookDirection.y = 0;
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemyConfigs.EnemyMoveSpeed * Time.deltaTime); //makes the enemy move towards the player
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && collision.gameObject.TryGetComponent(out IDamageable player) && enemyCurrentHP > 0)
        {
            enemyAnimations.SetTrigger("isAttacking");
            var playerHP = FirstPersonControls.Instance; // finds the FPC script on the player tag game object

            Vector3 direction = transform.forward * -enemyConfigs.EnemyKnockbackFactor;
            player.DamageReceived(enemyConfigs.EnemyAttackDamage.GetEnemyDamage());
            StartCoroutine(playerHP.KnockedBack(direction));
        }
    }

    public void DamageReceived(int damage)
    {
        enemyCurrentHP -= damage;
        UpdateEnemyHealthBar();
        DamageAndDeath();
        OnDamageReceived?.Invoke();
    }

    public void BreakStance()
    {
        Vector3 direction = -transform.forward;
        //direction.Normalize();
        rb.AddForce(direction * 3, ForceMode.Impulse);        
    }

    public void SetMaxEnemyHP()
    {
        enemyCurrentHP = enemyConfigs.MaxEnemyHP;
        enemyHealth.maxValue = enemyConfigs.MaxEnemyHP;
        UpdateEnemyHealthBar();
    }

    public void DamageAndDeath()
    {
        if (enemyCurrentHP > 0)
        {
            //BreakStance();
            StartCoroutine(enemyConfigs.DealDamage(enemyAnimations, transform));

        }
        else if (enemyCurrentHP <= 0)
        {
            enemySFX.Stop();
            enemySFX.clip = enemyConfigs.DeathSFX;
            enemyAnimations.SetTrigger("isDead");
            StartCoroutine(enemyConfigs.EnemyDeath(gameObject, enemySFX));
        }

    }

    public void UpdateEnemyHealthBar()
    {
        enemyHealth.value = enemyCurrentHP;
    }

    public void OnDisable()
    {
        OnDamageReceived = null;
        rb = null;
    }
}
