using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleScriptableObject", menuName = "Scriptable Objects/ObstacleScriptableObject")]
public class ObstacleScriptableObject : ScriptableObject
{
    [Tooltip("The sprite asset this Obstacle uses")]
    public Sprite obstacleSprite;

    [Tooltip("If true, this Obstacle can be destroyed with Player projectiles.")]
    public bool isDestructible = true;

    [Tooltip("If true, this Obstacle will spawn floating in the air.")]
    public bool isFloating = false;

    [Tooltip("The scale of this Obstacle. (1.5x, 1.5y) by default")]
    public Vector2 obstacleScale = new Vector2(1.5f, 1.5f);

}
