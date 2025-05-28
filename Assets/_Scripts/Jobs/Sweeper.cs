using UnityEngine;

public class Sweeper : BaseJob
{
    public override void InitState(JobManager ctx)
    {
        base.InitState(ctx);
        jobState = JobManager.JobState.Sweeper;
    }
    public override void DoYerJob(PlayerController player)
    {
        Debug.Log("Sweeper");
    }
    public override void StartJob()
    {
        base.StartJob();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
