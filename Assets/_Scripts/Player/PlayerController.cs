using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] float xClampPercentage = 80;
    [SerializeField] float yClampPercentage = 80;
    [SerializeField] float xMinOffset;
    [SerializeField] float xMaxOffSet;
    [SerializeField] float yMinOffset;
    [SerializeField] float yMaxOffSet;


    bool isMoving;

    InputAction move;
    PlayerInput playerInput;
    public Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions.FindAction("Movement");
    }

    public void OnMoveStarted(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        isMoving = true;
        Debug.Log("Is Moving");
    }
    public void OnMoveStopped(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        isMoving = false;
        Debug.Log("Stopped Moving");
    }
    
    void Update()
    {
        if (isMoving)
        { 
            Vector2 movementInput = move.ReadValue<Vector2>();
            Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0);
            transform.Translate(movement * moveSpeed * Time.deltaTime);

            ClampPosition();
        }
    }

    void ClampPosition()
    {
        float camHeight = _camera.orthographicSize;
        float camWidth = camHeight * _camera.aspect;

        float xMin = -camWidth * xClampPercentage * xMinOffset;
        float xMax = camWidth * xClampPercentage * xMaxOffSet;
        float yMin = -camHeight * yClampPercentage * yMinOffset;
        float yMax = camHeight * yClampPercentage * yMaxOffSet;

        Vector2 clampPos = transform.position;

        clampPos.x = Mathf.Clamp(clampPos.x, xMin, xMax);
        clampPos.y = Mathf.Clamp(clampPos.y, yMin, yMax);

        transform.position = clampPos;
    }
}
