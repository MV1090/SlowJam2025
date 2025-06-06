using UnityEngine;
using System.Collections.Generic;

/* Level Details
 * This is data the Level Manager uses to spawn Encounters and Random Obstacles.
 * Use these to add unique personality to each level.
 */

[CreateAssetMenu(fileName = "LevelDetailsScriptableObject", menuName = "Scriptable Objects/LevelDetailsScriptableObject")]
public class LevelDetailsScriptableObject : ScriptableObject
{
    [Header("Details")]
    [Tooltip("If true, this will pick a random Encounter. Otherwise, it will spawn in the order of the List.")]
    public bool pickRandomly = true;

    [Header("ScriptableObjects")]
    [Tooltip("This is all the possible encounters this level can spawn.")]
    public List<LevelEncounterScriptableObject> levelEncounters;

    [Tooltip("Random obstacles that can spawn between encounters during the level.")]
    public List<ObstacleScriptableObject> levelRandomObstacles;

    [Tooltip("Stores the type of Customers that may appear in this level.")]
    public ObstacleScriptableObject levelCustomers;

}
