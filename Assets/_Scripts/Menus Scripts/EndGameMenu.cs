using TMPro;
using UnityEngine;

public class EndGameMenu : BaseMenu
{
    [SerializeField] TMP_Text score;
    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.EndGameMenu;
    }

    public override void EnterState()
    {
        base.EnterState();
        score.text = "SCORE: " + GameManager.Instance.Score;
        Time.timeScale = 0.0f;

        // Remove obstacles from the level
        LevelManager.LevelInstance.ClearAllObstacles();
    }

    public override void ExitState()
    {
        base.ExitState();
        GameManager.Instance.ResetGame();
        Time.timeScale = 1.0f;
        
    }

    public void JumpToMainMenu()
    {
        context.SetActiveMenu(MenuManager.MenuStates.MainMenu);
    }
    public void Replay()
    {
        context.SetActiveMenu(MenuManager.MenuStates.GameMenu);
        LevelManager.LevelInstance.StartNewGame(); // LevelManager must be active before it starts the game!

    }
}
