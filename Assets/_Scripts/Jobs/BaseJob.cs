using UnityEngine;

public class BaseJob : MonoBehaviour
{    
    public JobManager.JobState jobState;
    protected JobManager context;

    public virtual void InitState(JobManager ctx)
    {
        context = ctx;
    }

    public virtual void DoYerJob(PlayerController player) 
    {

    }

    public virtual void StartJob()
    {
        context.ActivateNewJob(jobState);
    }
    //protected virtual void EnterState()
    //{

    //}
    //protected virtual void ExitState()
    //{

    //}
}
