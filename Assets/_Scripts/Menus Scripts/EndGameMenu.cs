using UnityEngine;

public class EndGameMenu : BaseMenu
{
    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.EndGameMenu;
    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0.0f;
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1.0f;
    }
    public void JumpToMainMenu()
    {
        context.SetActiveMenu(MenuManager.MenuStates.MainMenu);
    }
    public void Replay()
    {
        context.SetActiveMenu(MenuManager.MenuStates.GameMenu);
    }
}
