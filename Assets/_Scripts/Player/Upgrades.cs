using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrades
{
    public string upgradeName;
    public int baseCost;
    public float costMultiplier;
    public List<float> levelValues;    

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
    }
}
