using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player Stats
    public int maxHealth = 3;
    public float projectileSpeed = 10f;
    private int health;

    // Animation
    public Animator animator;
    
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
    [SerializeField] private Camera _camera;

    bool isMoving;

    // Firing Settings     
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform  projectileSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
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

    public void OnDoingYerJob(InputAction.CallbackContext context)
    {
        JobManager.Instance.currentJob.DoYerJob(this);
    }

    public void OnFire(InputAction.CallbackContext context)
    {  
        if (!gameObject.activeInHierarchy)
            return;

        int playerLayer = LayerMask.NameToLayer("Player");
        int ignoreLayer = ~(1 << playerLayer);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, ignoreLayer))
        {
            targetPoint = hit.point;
            Debug.DrawLine(ray.origin, targetPoint, Color.red, 2f);
            Debug.Log("Hit point: " + targetPoint);
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
            Debug.DrawLine(ray.origin, targetPoint, Color.yellow, 2f);
            Debug.Log("No hit � firing into empty space.");
        }       
        Vector3 direction = (targetPoint - transform.position).normalized;

        //Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectile = ObjectPool.SharedInstance.GetProjectileObject();
        projectile.SetupProjectile(transform, 1.0f, projectileSpeed, direction, true);

        Debug.DrawLine(transform.position, targetPoint, Color.green, 2f);        
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

            animator.SetFloat("Hspeed", movementInput.x);

            ClampPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            print("Ouch! The player took some damage from a Projectile!");
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            print("Ouch! The player took some damage hitting an Obstacle!");
            other.gameObject.SetActive(false);
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

    public void SetHealthRelative(int healthValue)
    {
        health += Mathf.Clamp(health+healthValue, 0, maxHealth);
    }
}
