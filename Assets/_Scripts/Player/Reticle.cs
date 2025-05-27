using UnityEngine;
using UnityEngine.InputSystem;

public class MouseReticle : MonoBehaviour
{
    public Camera _camera;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();    
    }

    private void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldMousePosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.nearClipPlane));
        
        transform.position = worldMousePosition;               
    }
}
