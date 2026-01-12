using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    IReadOnlyList<Vector2Int> patrolRoute;
    int baseMoveBudget;
    int buffMoveBudget;
}
