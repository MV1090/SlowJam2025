using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Upgrades speedUpgrade;
    public Upgrades healthUpgrade;
    public Upgrades ammoUpgrade;

    private float playerCurrency;

    private Dictionary<string, Upgrades> upgrades;

    private void Start()
    {
        upgrades = new Dictionary<string, Upgrades>()
        {
            {"Speed", speedUpgrade},
            {"Health", healthUpgrade},
            {"Ammo", ammoUpgrade}
        };
    }

    private void TryUpgrade(string upgradeType)
    {
        if (upgrades.TryGetValue(upgradeType.ToLower(), out Upgrades upgrade))
        {
            if (upgrade.CanUpgrade(GameManager.Instance.money))
            {
                GameManager.Instance.money -= upgrade.CurrentCost;
                upgrade.ApplyUpgrade();

                CompleteUpgrade(upgradeType);

                Debug.Log($"Upgraded {upgradeType} to level {upgrade.CurrentLevel}. Value: {upgrade.CurrentValue}");
            }
        }
    }

    private void CompleteUpgrade(string upgradeType)
    {
        switch (upgradeType.ToLower())
        {
            case "speed":
                
                break;
            case "health":
                
                break;
            case "ammo":
                
                break;
        }
    }

    public void UpgradeSpeed() => TryUpgrade("speed");
    public void UpgradeHealth() => TryUpgrade("health");
    public void UpgradeAmmo() => TryUpgrade("ammo");


}
