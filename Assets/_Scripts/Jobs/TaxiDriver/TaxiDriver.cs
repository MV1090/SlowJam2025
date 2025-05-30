using Unity.VisualScripting;
using UnityEngine;

public class TaxiDriver : BaseJob
{
    bool hasCustomer;

    public PlayerController playerController;
    [SerializeField] Vector3 cubePosOffset;
    [SerializeField] Vector3 cubeSizeOffset;

    Customer customer;

    public override void InitState(JobManager ctx)
    {
        base.InitState(ctx);
        jobState = JobManager.JobState.TaxiDriver;

        cubePosOffset = new Vector3(0, 0, 1.25f);
        cubeSizeOffset = new Vector3(5, 3, 2.5f);
        hasCustomer = false;
    }
    public override void DoYerJob(PlayerController player)
    {
        if(!hasCustomer)
        {
            PickUpCustomer(player.transform);
            return;
        }
        else
        {
            DropOffCustomer();
            return;
        }
    }
    public override void StartJob()
    {
        base.StartJob();
    }

    private void PickUpCustomer(Transform playerTransform)
    {
        Debug.Log("Pick Up Customer");

        int customerLayer = 1 << LayerMask.NameToLayer("Customer");

        Collider[] hitCollider = Physics.OverlapBox(playerTransform.position + cubePosOffset, cubeSizeOffset, playerTransform.rotation, customerLayer);

        foreach (var hit in hitCollider)
        {
            if (!hit.CompareTag("Customer"))
                return;

            customer = hit.gameObject.GetComponent<Customer>();
            customer.pickedUP?.Invoke();
            customer.playerPos = playerTransform;

            hasCustomer = true;
        }     
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawCube(playerController.transform.position + cubePosOffset, cubeSizeOffset);
    //}

    private void DropOffCustomer()
    {
        customer.droppedOff?.Invoke();

        hasCustomer = false;
    }

}
