using UnityEngine;

[CreateAssetMenu(fileName = "LevelEncounterScriptableObject", menuName = "Scriptable Objects/LevelEncounterScriptableObject")]
public class LevelEncounterScriptableObject : ScriptableObject
{
    [Tooltip("What object should be spawned?")]
    public ObstacleScriptableObject objectToSpawn;

    [Tooltip("How many of these objects should spawn this encounter?")]
    public int amountToSpawn;

    [Tooltip("How quickly should Objects be spawned during this encounter?")]
    public float spawnRate;
}
