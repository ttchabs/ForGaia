using UnityEngine;

[System.Serializable]
public struct WeaponDamage
{
    public int minDamage;
    public int maxDamage;

    public WeaponDamage(int min, int max)
    {
        minDamage = min; 
        maxDamage = max;
    }

    public int GetRandomDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }

}

[System.Serializable]
public struct ExplosiveDamage
{
    public bool isExplosive;
    public float range;
}