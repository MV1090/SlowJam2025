using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    private void Start()
    {
        GameManager.Instance.OnScoreChanged.AddListener(UpdateScoreText);
    }

    private void UpdateScoreText(int value)
    {
        scoreText.text = "Score: " + value.ToString();
    }
}
