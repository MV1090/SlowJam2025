using UnityEngine;

public class JobTutorial : BaseMenu
{
    public GameObject deliveryTutorial;
    public GameObject taxiTutorial;
    public GameObject sweeperTutorial;

    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.JobTutorial;
    }

    public override void EnterState()
    {
        base.EnterState();
        SetJobTutorialText(JobManager.Instance.currentJob.jobState);
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
        
    }

    public void SetJobTutorialText(JobManager.JobState newState)
    {
        deliveryTutorial.SetActive(false);
        taxiTutorial.SetActive(false);
        sweeperTutorial.SetActive(false);

        switch (newState)
        {
            case JobManager.JobState.Delivery:
                deliveryTutorial.SetActive(true);
                break;
            case JobManager.JobState.TaxiDriver:
                taxiTutorial.SetActive(true);
                break;
            case JobManager.JobState.Sweeper:
                sweeperTutorial.SetActive(true);
                break;
            default:
                break;
        }
    }
}
