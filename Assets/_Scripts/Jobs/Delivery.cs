using UnityEngine;

public class Delivery : BaseJob
{
    public override void InitState(JobManager ctx)
    {
        base.InitState(ctx);
        jobState = JobManager.JobState.Delivery;
    }

    public override void DoYerJob(PlayerController player)
    {
        Debug.Log("Delivery");
    }

    public override void StartJob()
    {
        base.StartJob();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
