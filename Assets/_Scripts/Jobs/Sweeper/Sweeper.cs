using UnityEngine;

public class Sweeper : BaseJob
{

    [SerializeField] Vector3 pickUpRange;

    public override void InitState(JobManager ctx)
    {
        base.InitState(ctx);
        jobState = JobManager.JobState.Sweeper;
        pickUpRange = new Vector3(3, 3, 2.5f);
    }
    public override void DoYerJob(PlayerController player)
    {        
        PickUpTrash(player.transform);
    }
    public override void StartJob()
    {
        base.StartJob();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PickUpTrash(Transform playTransform)
    {
        Debug.Log("Pick Up Trash!!");

        int trashLayer = 1 << LayerMask.NameToLayer("Trash");

        Collider[] hitCollider = Physics.OverlapBox(playTransform.position, pickUpRange, playTransform.rotation, trashLayer);
        
        foreach(var hit in hitCollider)
        {
            Debug.Log(hit.name);
            Destroy(hit.gameObject);
        }
    }

}
