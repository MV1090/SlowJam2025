using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;

    private void Start()
    {
        GameManager.Instance.OnMoneyChanged.AddListener(UpdateMoneyText);
    }

    private void UpdateMoneyText(int value)
    {
        moneyText.text = "Money: " + value.ToString();
    }
}
