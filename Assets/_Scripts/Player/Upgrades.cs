using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Upgrades
{
    public string upgradeName;
    public int baseCost;
    public float costMultiplier;
    public List<float> levelValues;

    public Image[] upgradeable;
    [SerializeField] Sprite notUpgraded;
    [SerializeField] Sprite upgraded;

    private int currentLevel = 0;

    public int CurrentLevel => currentLevel;

    public float CurrentValue => levelValues[Mathf.Min(currentLevel, levelValues.Count - 1)];

    public int CurrentCost => Mathf.FloorToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));

    public bool CanUpgrade(int playerCurrency)
    {
        return currentLevel < levelValues.Count -1 && playerCurrency >= CurrentCost;
    }

    public void ApplyUpgrade()
    {
        if (currentLevel < levelValues.Count - 1)
            currentLevel ++;

        for(int i = 0; i < currentLevel; i ++)
        {
            upgradeable[i].sprite = upgraded;
            upgradeable[i].GetComponentInChildren<TMP_Text>().enabled = false;
        }
    }

    public void ResetUpgrade()
    {
        currentLevel = 0;
        
        for(int i = 0; i < upgradeable.Length; i ++)
        {
            upgradeable[i].sprite = notUpgraded;
            upgradeable[i].GetComponentInChildren<TMP_Text>().enabled = true;
        }
    }
}
