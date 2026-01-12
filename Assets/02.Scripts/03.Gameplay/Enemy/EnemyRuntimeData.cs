using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Enemy;

public class EnemyRuntimeData : MonoBehaviour
{
    Vector2Int enemyPos;
    EnemyBoardMode enemyBoardMode;
    EnemyBoardState enemyBoardState;

    int routeIndex;
    int leftMoveBudget;
}
