using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health:")]
    [Space(5)]
    public int totalHP = 5;
    public string currentHP;

    private int _currentHP;

    public void Awake()
    {
        _currentHP = totalHP;
        currentHP = _currentHP.ToString();        
    }

    public void LoseHP(int reduceHP)
    {
        _currentHP -= reduceHP;
        currentHP = _currentHP.ToString();
        if (_currentHP > 0) 
        {
            Debug.Log($"{currentHP}");
        }
        else
        {
            KillCharacter();
        }
    }

    public void GainHP(int increaseHP)
    {
        if (_currentHP < totalHP)
        {
            _currentHP += increaseHP;
            if (_currentHP > totalHP)
                _currentHP = totalHP;
            
            currentHP = _currentHP.ToString();
        }
        else
        {
            Debug.Log("Max HP");
        }
    }

    public void KillCharacter()
    {
        Debug.Log("Dead");
    }
}
