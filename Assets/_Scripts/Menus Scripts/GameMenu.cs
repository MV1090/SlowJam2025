using UnityEngine;
using UnityEngine.UI;

public class GameMenu : BaseMenu
{
    [SerializeField] GameObject upgrades;
    [SerializeField] Health playerHealth;
    [SerializeField] Slider healthBar;
    [SerializeField] Image jobIcon;
    [SerializeField] Sprite[] jobIcons;
    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.GameMenu;
        GameManager.Instance.OnLevelChanged.AddListener(ShowUpgrades);
        upgrades.SetActive(false);

        playerHealth.OnHealthChanged.AddListener(UpdateHealthBar);

        JobManager.Instance.jobChanged += UpdateImage; 
    }

    private void UpdateImage()
    {
        int jobIndex  = JobManager.Instance.JobIndex;

        jobIcon.sprite = jobIcons[jobIndex];
    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 1.0f;      
        
        if(!GameManager.Instance.hasPlayed)
            GameManager.Instance.hasPlayed = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        playerHealth.ResetHealth();
        Time.timeScale = 0.0f;
    }

    //Function for testing menu loop
    public void EndGame()
    {
        context.SetActiveMenu(MenuManager.MenuStates.EndGameMenu);
    }

    private void ShowUpgrades(int value) 
    {
        if (value > 1) 
        {
            upgrades.gameObject.SetActive(true);     
            AudioManager.Instance.StopAllSFX();
            Time.timeScale = 0.0f;
        }
    }
    public void HideUpgrades()
    {
        upgrades.gameObject.SetActive(false);
        AudioManager.Instance.PlayMovementSound();
        Time.timeScale = 1.0f;
    }
    void UpdateHealthBar(float health)
    {        
        float min = 0f;
        float max = GameManager.Instance.PlayerHealth;

        float normalizedHealth = (health - min) / (max - min);

        Debug.Log(normalizedHealth.ToString());

        healthBar.value = normalizedHealth;   
    }
}
