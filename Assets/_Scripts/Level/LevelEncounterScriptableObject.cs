using UnityEngine;
public enum EncounterType { Obstacles, Enemies, Special, RestStop }; // This should be kept in a separate file if more globals like this are to be made.

[CreateAssetMenu(fileName = "LevelEncounterScriptableObject", menuName = "Scriptable Objects/LevelEncounterScriptableObject")]
public class LevelEncounterScriptableObject : ScriptableObject
{
    [Tooltip("Select the type of Encounter this is")]
    public EncounterType encounterType = EncounterType.Obstacles;

    [Tooltip("What object should be spawned?")]
    public ObstacleScriptableObject objectToSpawn;

    [Tooltip("How many of these objects should spawn this encounter?")]
    public int amountToSpawn;

    [Tooltip("How quickly should Objects be spawned during this encounter?")]
    public float spawnRate;
}
