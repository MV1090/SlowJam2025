using UnityEngine;

public class GameMenu : BaseMenu
{
    [SerializeField] GameObject upgrades;
    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.GameMenu;
        GameManager.Instance.OnLevelChanged.AddListener(ShowUpgrades);
        upgrades.SetActive(false);
    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 1.0f;
    }

    public override void ExitState()
    {
        base.ExitState();
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
            Time.timeScale = 0.0f;
        }
    }
    public void HideUpgrades()
    {
        upgrades.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
