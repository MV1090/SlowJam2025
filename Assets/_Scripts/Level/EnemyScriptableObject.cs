using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/EnemyScriptableObject")]
public class EnemyScriptableObject : ObstacleScriptableObject
{
    [Tooltip("Animator reference to this enemy's animations.")]
    public RuntimeAnimatorController enemyAnim;

    [Tooltip("The max health this enemy has.")]
    public int health = 5;

    [Tooltip("Additonal speed relative to the World Speed.")]
    public float relativeSpeed = 0.0f;

}
