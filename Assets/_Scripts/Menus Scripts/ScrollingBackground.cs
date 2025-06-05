using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeedX = 0.1f;    
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offset.x += scrollSpeedX * Time.unscaledDeltaTime;       
        mat.mainTextureOffset = offset;
    }
}
