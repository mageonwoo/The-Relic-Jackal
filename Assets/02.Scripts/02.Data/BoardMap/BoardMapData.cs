using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardMapData", menuName = "Maps/MapData")]
public class BoardMapData : ScriptableObject
{
    public List<Vector2Int> obstacles;
    public List<Vector2Int> traps;

    public List<Vector2Int> enemySpawns;

    public Vector2Int playerSpawn;
    public Vector2Int GoalPoint;
}
