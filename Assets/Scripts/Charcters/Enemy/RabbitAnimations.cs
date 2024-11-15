using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAnimations : MonoBehaviour
{

    public Animator rabbitanim;
    public bool isMoving;
    public float moveSPeed = 0.1f;
    public float distance;
    public Transform player;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > moveSPeed || Mathf.Abs(Input.GetAxis("Vertical")) > moveSPeed;
        rabbitanim.SetTrigger("IsMoving");

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distance > distanceToPlayer)
        {
            Attack();
        }
        
        else 
        {
            Still();   
        }
    }

    void Attack() 
    {
        rabbitanim.SetTrigger("Attack");
    }


    public void LookAtPlayer()
    {
        Vector3 directioToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directioToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    public void Still() 
    {
        rabbitanim.SetTrigger("IsStill");
    }
}

