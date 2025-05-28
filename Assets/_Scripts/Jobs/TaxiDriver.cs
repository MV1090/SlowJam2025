using UnityEngine;

public class TaxiDriver : BaseJob
{
    public override void InitState(JobManager ctx)
    {
        base.InitState(ctx);
        jobState = JobManager.JobState.TaxiDriver;
    }
    public override void DoYerJob(PlayerController player)
    {
        Debug.Log("TaxiDriver");
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
