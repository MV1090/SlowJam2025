using UnityEngine;

public class HowToPlay : BaseMenu
{

    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.HowToPlay;
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

    public void JumpToGameMenu()
    {
        context.SetActiveMenu(MenuManager.MenuStates.GameMenu);
        LevelManager.LevelInstance.StartNewGame();
        AudioManager.Instance.PlayBackgroundTrack();
    }
}
