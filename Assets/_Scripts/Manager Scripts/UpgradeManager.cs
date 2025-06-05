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
            {"speed", speedUpgrade},
            {"health", healthUpgrade},
            {"ammo", ammoUpgrade}
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

                CompleteUpgrade(upgradeType, upgrade);

                Debug.Log($"Upgraded {upgradeType} to level {upgrade.CurrentLevel}. Value: {upgrade.CurrentValue}");
            }
            else { Debug.Log("no money Failed"); }


        }
        else
        {
            Debug.Log("upgrade Failed");
        }

    }

    private void CompleteUpgrade(string upgradeType, Upgrades upgrade)
    {
        switch (upgradeType.ToLower())
        {
            case "speed":
                GameManager.Instance.PlayerSpeed = upgrade.CurrentValue;
                Debug.Log("Upgrade speed");
                break;
            case "health":
                Debug.Log("Upgrade health");
                break;
            case "ammo":
                Debug.Log("Upgrade ammo");
                break;
        }
    }

    public void UpgradeSpeed() => TryUpgrade("speed");
    public void UpgradeHealth() => TryUpgrade("health");
    public void UpgradeAmmo() => TryUpgrade("ammo");


}
