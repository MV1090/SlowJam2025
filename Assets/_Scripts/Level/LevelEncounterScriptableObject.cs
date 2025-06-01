using UnityEngine;
public enum EncounterType // This should be kept in a separate file if more globals like this are to be made.
{
    [InspectorName("Spawn Obstacle")]
    Obstacles,
    [InspectorName("Spawn Enemy")]
    Enemies,
    [InspectorName("Spawn Objective")]
    Special,
    [InspectorName("Spawn Rest Stop")]
    RestStop 
}; 

[CreateAssetMenu(fileName = "LevelEncounterScriptableObject", menuName = "Scriptable Objects/LevelEncounterScriptableObject")]
public class LevelEncounterScriptableObject : ScriptableObject
{
    [Tooltip("The type of Encounter this is. Determines what ScriptableObject the Level Manager requires.")]
    public EncounterType encounterType = EncounterType.Obstacles;

    [Tooltip("The ScriptableObject related to this Encounter")]
    public ObstacleScriptableObject objectToSpawn;

    [Tooltip("How many of these objects should spawn this encounter?")]
    public int amountToSpawn;

    [Tooltip("How quickly should Objects be spawned during this encounter?")]
    public float spawnRate;
}
