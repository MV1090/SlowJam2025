using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleScriptableObject", menuName = "Scriptable Objects/ObstacleScriptableObject")]
public class ObstacleScriptableObject : ScriptableObject
{
    [Tooltip("The sprite asset this Obstacle uses")]
    public Sprite obstacleSprite;

    [Tooltip("Can this Obstacle be destroyed with player gunfire? (true) by default")]
    public bool isDestructible = true;

    [Tooltip("Is this Obstacle meant to be floating? (false) by default")]
    public bool isFloating = false;

    [Tooltip("The scale of this Obstacle. (1.5x, 1.5y) by default")]
    public Vector2 obstacleScale = new Vector2(1.5f, 1.5f);

}
