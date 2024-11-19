using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public struct WeaponDamage
{
    public int minDamage;
    public int maxDamage;
    public float critMultiplier;
    [Range (0f, 1f)] public float critChance;


    public WeaponDamage(int min, int max, float chance, float multiplier)
    {
        minDamage = min; 
        maxDamage = max;
        critChance = chance;
        critMultiplier = multiplier;
    }

    public readonly int GetDamage()
    {
        float damage = Random.Range(minDamage, maxDamage + 1);
        if (Random.value < critChance)
        {
            return Mathf.RoundToInt(damage *= critMultiplier);
        }
        else
        {
            return Mathf.RoundToInt(damage);
        }
    }
}

[System.Serializable]
public struct EnemyDamage
{
    public int minDamage;
    public int maxDamage;

    public EnemyDamage(int minD, int maxD)
    {
        minDamage = minD;
        maxDamage = maxD;
    }

    public readonly int GetEnemyDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }
}

