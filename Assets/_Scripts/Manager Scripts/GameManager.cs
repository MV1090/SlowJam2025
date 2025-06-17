using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public bool hasPlayed = false;

    // TODO: These could be set as an array if there's enough tutorials
    public bool seenDeliveryTutorial = false;
    public bool seenTaxiTutorial = false;
    public bool seenSweeperTutorial = false;

    public bool hasTurboMode = true;

    public Action OnDeath;

    private int _money = 0;
    public UnityEvent<int> OnMoneyChanged;
    public int money
    {
        get => _money;

        set
        {
            _money = value;
            OnMoneyChanged?.Invoke(_money);
            Debug.Log("Money: " + _money);
        }
    }

    private int _score = 0;
    public UnityEvent<int> OnScoreChanged;
    public int Score
    {
        get => _score;

        set
        {
            _score = value;
            OnScoreChanged?.Invoke(_score);
            Debug.Log("Score: " + _score);
        }
    }

    private int _level = 0;
    public UnityEvent<int> OnLevelChanged;
    public int Level
    {
        get => _level;

        set
        {
            _level = value;
            OnLevelChanged?.Invoke(_level);
            //Debug.Log("Welcome to Level " + _level);
        }
    }

    [SerializeField] private float _playerSpeed = 3;
    public float PlayerSpeed
    {
        get => _playerSpeed;

        set
        {
            _playerSpeed = value;
        }
    }

    [SerializeField] private float _playerHealth = 100;
    public UnityEvent<float> PlayerHealthChanged;
    public float PlayerHealth
    {
        get => _playerHealth;

        set
        {
            _playerHealth = value;
            PlayerHealthChanged?.Invoke(_playerHealth);
        }
    }

    [SerializeField] private float _projectileAOE = 0;
    public float ProjectileAOE
    {
        get => _projectileAOE;

        set
        {
            _projectileAOE = value;
        }
    }

    public UnityEvent<string> OnJobChanged;

    private void Start()
    {
        // Play background music when the game starts
       AudioManager.Instance.PlayBackgroundTrack();
    }

    public void ChangeJobDescription(string jobDescriptor)
    {
        OnJobChanged?.Invoke(jobDescriptor);
    }

    public void ResetGame()
    {
        ProjectileAOE = 0;
        PlayerHealth = 100;
        PlayerSpeed = 3;
        Level = 0;
        Score = 0;
        money = 0;
    }

    /**
     * Makes the tutorial screen for a respective job appear, if it hasn't appeared already.
     * Mainly called by the Level Manager when a job is selected.
     */
    public void ActivateJobTutorial(JobManager.JobState newState)
    {
        switch (newState)
        {
            case JobManager.JobState.Delivery:
                if (!seenDeliveryTutorial)
                {
                    MenuManager.Instance.SetActiveMenu(MenuManager.MenuStates.JobTutorial);
                    seenDeliveryTutorial = true;
                }
                break;
            case JobManager.JobState.TaxiDriver:
                if(!seenTaxiTutorial)
                {
                    MenuManager.Instance.SetActiveMenu(MenuManager.MenuStates.JobTutorial);
                    seenTaxiTutorial = true;

                }
                break;
            case JobManager.JobState.Sweeper:
                if (!seenSweeperTutorial)
                {
                    MenuManager.Instance.SetActiveMenu(MenuManager.MenuStates.JobTutorial);
                    seenSweeperTutorial = true;

                }
                break;
            default:
                break;
        }
    }

    /**
     * Toggles if Turbo mode is enabled or disabled.
     * Mainly called by the Toggle UI in the Main Menu through a Unity Event.
     */
    public void ToggleTurboMode()
    {
        hasTurboMode = !hasTurboMode;
        print("Turbo Mode: " + hasTurboMode);
    }
}
