using UnityEngine;

/** ObjectShadow.cs
 *  Follows the Object this is attached to. The shadow can be placed at any 
 *  arbitrary y position and scales based on the object's y distance from the shadow.
 * 
 */

public class ObjectShadow : MonoBehaviour
{
    public Transform targetTransform;
    public float shadowScaleMin = 0.25f;
    public float shadowYPosition = 0.13f;
    private Vector3 shadowOriginalScale;
    private float shadowOffsetFromTarget;

    private void Awake()
    {
        if (targetTransform == null) //attempt to fetch parent transform
            targetTransform = gameObject.GetComponentInParent<Transform>();
    }

    private void Start()
    {
        shadowOffsetFromTarget = Mathf.Abs(gameObject.transform.position.y - targetTransform.position.y);
        shadowOriginalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Update shadow position based on target positon and shadow offset
        gameObject.transform.position = new Vector3(targetTransform.position.x, shadowYPosition, targetTransform.position.z);

        // Scale shadow based on distance from Target
        float shadowScale = Mathf.Lerp(1.0f, shadowScaleMin, targetTransform.position.y - gameObject.transform.position.y - shadowOffsetFromTarget);
        gameObject.transform.localScale = new Vector3(shadowOriginalScale.x * shadowScale, shadowOriginalScale.y * shadowScale, shadowOriginalScale.z * shadowScale);
    }
}
