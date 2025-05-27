using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement Settings
    public float moveSpeed = 5f;
    [SerializeField] float xClampPercentage = 80;
    [SerializeField] float yClampPercentage = 80;
    [SerializeField] float xMinOffset;
    [SerializeField] float xMaxOffSet;
    [SerializeField] float yMinOffset;
    [SerializeField] float yMaxOffSet;

    InputAction move;
    PlayerInput playerInput;
    public Camera _camera;     

    bool isMoving;

    // Firing Settings
    [SerializeField] private GameObject reticle;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform  projectileSpawn;

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
    }
    public void OnMoveStopped(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        isMoving = false;        
    }

    public void OnFire(InputAction.CallbackContext context)
    {  
        if (!gameObject.activeInHierarchy)
            return;

        Projectile projectile = Instantiate(projectilePrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
        projectile.targetPosition = new Vector3(reticle.transform.position.x, reticle.transform.position.y, 0);

        Debug.Log("POW POW POW POW");
    }
    
    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

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
