using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player Stats
    //public int maxHealth = 3;
    public float projectileSpeed = 10f;
    //private int health;

    // Animation
    public Animator animator;
    
    // Movement Settings
    //public float moveSpeed = 5f;
    [SerializeField] float xClampPercentage = 80;
    [SerializeField] float yClampPercentage = 80;
    [SerializeField] float xMinOffset;
    [SerializeField] float xMaxOffSet;
    [SerializeField] float yMinOffset;
    [SerializeField] float yMaxOffSet;
    [SerializeField] ParticleSystem jetParticles;

    InputAction move;
    PlayerInput playerInput;
    [SerializeField] private Camera _camera;
    Vector2 movementInput;

    bool isMoving;

    // Firing Settings     
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform  projectileSpawn;
    [SerializeField] private Transform  projectileSpawnLeft;
    [SerializeField] private Transform  projectileSpawnRight;
    [SerializeField] private Transform  playerSpawnPoint;

    Projectile.ProjectileType projectileType = Projectile.ProjectileType.Player;

    Health healthComponent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //health = maxHealth;
        isMoving = false;
        playerInput = GetComponent<PlayerInput>();
        move = playerInput.actions.FindAction("Movement");
        healthComponent = GetComponent<Health>();

        GameManager.Instance.OnDeath += Respawn;
    }

    void Respawn()
    {
        transform.position = playerSpawnPoint.position;
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
        movementInput.x = 0;
        animator.SetFloat("Hspeed", movementInput.x);
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
            //Debug.Log("Hit point: " + targetPoint);
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
            Debug.DrawLine(ray.origin, targetPoint, Color.yellow, 2f);
            //Debug.Log("No hit � firing into empty space.");
        }       
        Vector3 direction = (targetPoint - transform.position).normalized;

        Transform spawnPoint;

        if (movementInput.x > 0)
            spawnPoint = projectileSpawnRight;
        else if (movementInput.x < 0) 
            spawnPoint = projectileSpawnLeft;
        else 
            spawnPoint = projectileSpawn;

        //Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectile = ObjectPool.SharedInstance.GetProjectileObject();
        projectile.SetupProjectile(spawnPoint, 1.0f, projectileSpeed, direction, projectileType);

        Debug.DrawLine(transform.position, targetPoint, Color.green, 2f);

        animator.SetTrigger("Shooting");

        // Play shooting sound effect
        AudioManager.Instance.PlayShootingSoundEffect();
    }
    
    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (isMoving)
        { 
            /*Vector2*/ movementInput = move.ReadValue<Vector2>();
            Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0);
            transform.Translate(movement * GameManager.Instance.PlayerSpeed * Time.deltaTime);

            animator.SetFloat("Hspeed", movementInput.x);
            animator.SetFloat("Vpos", transform.position.y);

            ClampPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            print("Ouch! The player took some damage from a Projectile!");
            other.gameObject.SetActive(false);
            healthComponent.TakeDamage(10.0f);
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            WorldObstacle obstacle = other.gameObject.GetComponent<WorldObstacle>();

            if (obstacle.obstacleType == ObstacleType.EndLevel)
            {
                print("The Level has Ended.");
                LevelManager.LevelInstance.SetUpNewLevel();
            }
            else
            {
                print("Ouch! The player took some damage hitting an Obstacle!");
                other.gameObject.SetActive(false);
                healthComponent.TakeDamage(15.0f);
            }
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

    public void TurnOffParticles()
    {
        if (jetParticles.isStopped)
            return;

        AudioManager.Instance.PlayFootstepsSoundEffect();
        jetParticles.Stop();
    }

    public void TurnOnParticles()
    {
        if (jetParticles.isPlaying)
            return;

        AudioManager.Instance.PlayJetpackOnSoundEffect();
        jetParticles.Play();
    }
}
