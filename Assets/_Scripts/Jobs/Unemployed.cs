using UnityEngine;

public class Unemployed : BaseJob
{
    public override void InitState(JobManager ctx)
    {
        base.InitState(ctx);
        jobState = JobManager.JobState.Unemployed;
    }
    public override void DoYerJob(PlayerController player)
    {
        Debug.Log("Unemployed");
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
