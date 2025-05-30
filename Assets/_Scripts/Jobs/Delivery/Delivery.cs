using UnityEngine;

public class Delivery : BaseJob
{
    Vector3 throwDirection;

    int numOfDeliveries;
    [SerializeField] int foodToDeliver = 5;    

    int numOfShifts = 0;

    public Camera _camera;

    [SerializeField] FoodBag foodPrefab;
    public override void InitState(JobManager ctx)
    {
        base.InitState(ctx);
        jobState = JobManager.JobState.Delivery;                
    }

    public override void DoYerJob(PlayerController player)
    {
        Debug.Log("Delivery");
        ThrowFood(player.transform);        
    }

    public override void StartJob()
    {
        base.StartJob();
                       
        numOfDeliveries += foodToDeliver + numOfShifts;            
        
        numOfShifts++;
    }


    void ThrowFood(Transform player)
    {
        if (numOfDeliveries <= 0)
        {
            Debug.Log("Out of food");
            return;
        }

        int playerLayer = LayerMask.NameToLayer("Player");
        int ignoreLayer = ~(1 << playerLayer);

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100, ignoreLayer))
        {
            targetPoint = hit.point;
            Debug.DrawLine(ray.origin, targetPoint, Color.red, 2f);            
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
            Debug.DrawLine(ray.origin, targetPoint, Color.yellow, 2f);            
        }

        throwDirection = (targetPoint - player.position).normalized;

        FoodBag foodBag = Instantiate(foodPrefab, player.position, player.rotation);
        foodBag.direction = throwDirection;        

        Debug.Log("Food to deliver: " + numOfDeliveries);
        numOfDeliveries--;

        Debug.DrawLine(transform.position, targetPoint, Color.green, 2f);
    } 
       
}
