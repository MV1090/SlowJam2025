using UnityEngine;

public class MainMenu : BaseMenu
{
    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.MainMenu;                   
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

    public void JumpToSettingsMenu()
    {
        context.SetActiveMenu(MenuManager.MenuStates.SettingsMenu);
    }

    public void JumpToGameMenu()
    {
        context.SetActiveMenu(MenuManager.MenuStates.GameMenu);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
