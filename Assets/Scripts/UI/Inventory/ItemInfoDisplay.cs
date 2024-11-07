using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoDisplay : MonoBehaviour
{
    public static ItemInfoDisplay Instance;

    [Header("NormalDisplay")]
    public GameObject normalInfo;
    public TextMeshProUGUI normalName;
    public TextMeshProUGUI normalDescription;

    [Header("MeleeDisplay")]
    public GameObject meleeInfoPanel;
    public TextMeshProUGUI meleeName;
    public TextMeshProUGUI meleeDescription;
    public TextMeshProUGUI meleeDamage;
    public TextMeshProUGUI meleeKnockback;
    public TextMeshProUGUI meleeWeight;
    public TextMeshProUGUI meleeClass;
    public TextMeshProUGUI meleeSwingCooldown;

    [Header("RangeDisplay")]
    public GameObject rangeInfoPanel;
    public TextMeshProUGUI rangeName;
    public TextMeshProUGUI rangeDescription;
    public TextMeshProUGUI rangeDamage;
    public TextMeshProUGUI rangeBulletSpeed;
    public TextMeshProUGUI rangeWeight;
    public TextMeshProUGUI rangeClass;
    public TextMeshProUGUI rangeFireRate;
    public TextMeshProUGUI rangeMagSize;

    public void NormalDisplayFunction(PickUpScriptable pickUpInfo)
    {
        
    }

    public void MeleeDisplayFunction(PickUpScriptable pickUpInfo) { }
}
