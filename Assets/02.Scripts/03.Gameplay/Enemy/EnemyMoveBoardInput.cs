using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemy
{
    // 데이터를 받아들이는 객체 X
    // 외부를 보호하는 방어 레이어 X
    // 이미 검증·수집된 값을 [묶어서 전달]하는 컨테이너
    // Input은 [입력 처리기]가 아니라 [입력 결과물]
    public readonly struct EnemyMoveBoardInput
    {
        public readonly Vector2Int enemyPos;
        public readonly Vector2Int playerPos;
        public readonly bool playerInRange;

        public readonly IReadOnlyList<Vector2Int> PatrolRoute;
        public readonly int routeIndex;
        public readonly int moveBudget;
        public readonly EnemyBoardMode boardMode;
        public readonly EnemyBoardState boardState;

        // new로 새로 생성하는 것이 아니라 입력된 값을 대입만 함
        public EnemyMoveBoardInput(Vector2Int _enemyPos, Vector2Int _playerPos,
                                   bool _playerInRange, IReadOnlyList<Vector2Int> _PatrolRoute,
                                   int _routeIndex, int _moveBudget, EnemyBoardMode _boardMode, EnemyBoardState _boardState)

        {
            enemyPos = _enemyPos;
            playerPos = _playerPos;
            playerInRange = _playerInRange;
            PatrolRoute = _PatrolRoute;
            routeIndex = _routeIndex;
            moveBudget = _moveBudget;
            boardMode = _boardMode;
            boardState = _boardState;
        }
    }
    // [알아두면 좋은 개념]
    //
    // [static factory] (정적 팩토리)
    //  new EnemyMoveBoardInput()를 대신해 입력(Input) 객체 생성 책임에만 관여하는 개념으로,
    //
    //  [예시]
    //    public static EnemyMoveBoardInput Create( Vector2Int _enemyPos, Vector2Int _playerPos,
    //                                              bool _playerInRange, IReadOnlyList<Vector2Int> _PatrolRoute,
    //                                              int _routeIndex, int _moveBudget, EnemyBoardMode _boardMode, EnemyBoardState _boardState)
    //    {
    //        return new EnemyMoveBoardInput(
    //            _enemyPos, _playerPos, _playerInRange,
    //            _PatrolRoute, _routeIndex, _moveBudget,
    //            _boardMode, _boardState
    //        );
    //    }
    //
    //   이런 형태를 가진다.
    //   정적 팩토리는 생성자 시그니처가 길어질 때, 이름으로 의도를 표현할 때, 생성 규칙이 생길 때를 위한 것.
    //   로직의 안정성이 아니라 가독성을 위한 것이다.
}