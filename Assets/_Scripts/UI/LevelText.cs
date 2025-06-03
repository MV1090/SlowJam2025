using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text jobText;

    private void Start()
    {
        GameManager.Instance.OnLevelChanged.AddListener(UpdateLevelText);
        GameManager.Instance.OnJobChanged.AddListener(UpdateJobText);
        // The level manager updates these texts at Start, so they don't need to be activated here.
        // UpdateLevelText(1);
        // UpdateJobText("---");
    }

    private void UpdateLevelText(int value)
    {
        levelText.text = "Stage: " + value.ToString();
    }

    private void UpdateJobText(string newDescriptor)
    {
        jobText.text = newDescriptor;
    }
}
