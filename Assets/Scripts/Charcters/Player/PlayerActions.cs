using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public FirstPersonControls playerControls;

    #region MELEE ASPECTS:
    public void StartMeleeAttack()
    {
        playerControls.meleeAttacks.hitBox.enabled = true;

    }

    public void EndMeleeAttack()
    {  
        playerControls.meleeAttacks.hitBox.enabled = false;
    }
    #endregion

    #region GUN ASPECTS
    public void ReloadWeapon()
    {
        playerControls.gunFire.ReloadGun();  
    }
    #endregion
}
