using UnityEngine;

public class GameMenu : BaseMenu
{
    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.GameMenu;
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
}
