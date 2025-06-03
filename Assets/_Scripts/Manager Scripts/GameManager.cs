using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    private int _money = 0;
    public UnityEvent<int> OnMoneyChanged;
    public int money
    {
        get => _money;

        set
        {
            _money = value;
            OnMoneyChanged?.Invoke(_money);
            Debug.Log("Money: "+_money);
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
            Debug.Log("Score: "+_score);
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

    public UnityEvent<string> OnJobChanged;

    private void Start()
    {
        
    }    

    public void ChangeJobDescription(string jobDescriptor)
    {
        OnJobChanged?.Invoke(jobDescriptor);
    }
}
