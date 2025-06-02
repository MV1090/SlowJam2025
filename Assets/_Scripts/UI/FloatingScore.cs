using TMPro;
using UnityEngine;

public class FloatingScore : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;
    private TMP_Text text;
    private float timer;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer += Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

        if (timer >= fadeDuration)
            Destroy(gameObject);
    }
}

