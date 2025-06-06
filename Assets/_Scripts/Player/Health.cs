using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    public UnityEvent<float> OnHealthChanged;
    public float CurrentHealth
    {
        get => _currentHealth;

        set
        {
            _currentHealth = value;           
            OnHealthChanged?.Invoke(_currentHealth);
            Debug.Log("Health: " + _currentHealth);
        }
    }


    void Start()
    {
        CurrentHealth = GameManager.Instance.PlayerHealth;
        GameManager.Instance.PlayerHealthChanged.AddListener(HealthUpgraded);
        GameManager.Instance.OnLevelChanged.AddListener(RestoreHalfHealth);
    }

    public void RestoreHalfHealth(int value)
    {
        CurrentHealth += GameManager.Instance.PlayerHealth / 2;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, GameManager.Instance.PlayerHealth);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        IsDead(CurrentHealth);
    }

    void HealthUpgraded(float value)
    {
        ResetHealth();
    }

    void IsDead(float health)
    {
        if (health < 0)
        {
            GameManager.Instance.OnDeath?.Invoke();
        }
    }

    public void ResetHealth()
    {
        CurrentHealth = GameManager.Instance.PlayerHealth;
    }
}
