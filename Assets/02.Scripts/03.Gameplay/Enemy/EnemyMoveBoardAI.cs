using System.Collections;
using System.Collections.Generic;
using Gameplay.Board.PathCalculator;
using Gameplay.Enemy;
using UnityEngine;

// Input은 입력된 값 데이터를 캡처해 읽기 전용 복사본으로만 저장하는 스냅샷이다.
// AI는 Input을 내부에 저장하거나 수정하지 않고, Output은 return으로만 내보낸다.
//  ▼
// EnemyMoveBoardAI는 상태를 갖지 않는 계산 역할만 수행한다. 순수 함수 집합이다.
//
// 기억해야 할 전제사항
// 1. Input은 이미 검증된 값이다
// 2. patrolRoute가 비어있을 수 있다.
// 3. moveBudget은 0일 수 있다.
// 4. 경로가 없을 수도 있다.

// Input에는 스냅샷 생성자
// public EnemyMoveBoardInput(Vector2Int _enemyPos, Vector2Int _playerPos,
//                            bool _playerInRange, IReadOnlyList<Vector2Int> _PatrolRoute,
//                            int _routeIndex, int _moveBudget, EnemyBoardMode _boardMode, EnemyBoardState _boardState)

// Output에는 계산 결과값 보관용
// public EnemyMoveBoardOutput(Vector2Int _nextPos, int _nextRouteIndex, EnemyBoardMode _boardMode)
// {
//      this.nextPos = _nextPos;
//      this.nextRouteIndex = _nextRouteIndex;
//      this.boardMode = _boardMode;
// }
// 메서드가 각각 존재한다.
public class EnemyMoveBoardAI
{
    // 1. AI의 판단 절차.
    //  1) 현재 적 인스턴스의 상태 (enemyMode)
    //  2) 목표 결정 (patrol: patrolRoute / chase: playerPos)
    //  3) 이동 가능 여부 (moveBudget > 0 / enemyPos == 타겟의 위치)

    // 2. 실패를 분기로 만들지 않는다.
    //  AI관점에서 실패하는 상황은, 다음 위치를 현재 위치로 반환한다는 계산만 한다.
    //      ▼
    //  nextPos = enemyPos

    // 3. Output에는 최소한의 정보만 담는다.
    // nextPos / nextRouteIndex / nextBoardMode
    public EnemyMoveBoardOutput Calculate(EnemyMoveBoardInput boardInput)
    {
        // 1) 기본값(실패 기본 수렴) 세팅
        Vector2Int nextPos = boardInput.enemyPos;
        int nextRouteIndex = boardInput.routeIndex;
        EnemyBoardMode nextMode = boardInput.boardMode;
        EnemyBoardState nextState = boardInput.boardState;

        // 2) 조기 종료 조건(움직일 이유/능력이 없는 턴)
        // ‘이동 계산을 시도할 수 있는지’를 판단한다.
        // 이동 예산이 남아있지 않으면, if문에 진입하지 않는다.
        if (boardInput.moveBudget > 0)
        {
            // 3) 목표 좌표(target) 결정
            if (boardInput.playerInRange)
            {
                nextMode = EnemyBoardMode.Chase;
                // playerPos 추적
            }
            else
            {
                nextMode = EnemyBoardMode.Patrol;
                if (boardInput.PatrolRoute.Count > 0)
                {
                    // 4) 도착 처리(Patrol)
                    // 5) 경로 확인(갈 수 있는가 판단)
                    // 6) 다음 칸 후보(nextPosCandidate) 선정
                }
            }
            // 7) moveBudget 소비 판단
            // 8) 다음 상태(nextMode) 판단
            // 9) Output 생성해서 return
        }

        return new EnemyMoveBoardOutput(nextPos, nextRouteIndex, nextMode, nextState);
    }
}
// <중요 학습 상황>
// 순수 AI 계산을 위한 스크립트에
// 필드를 직접 참조해 가져오는 오류를 범했을 때
// 해결을 위한 사고 방향
// 1. Calculate()에 인자가 없다
// 2. 계산하려면 값이 필요하다
// 3. 그 값을 어디선가 꺼내야 한다
// 4. 결국 클래스 필드에 선언한다
// 5. AI가 상태를 가지기 시작한다
// 6. “순수 계산기”라는 설계가 무너진다

// Q: 그럼 어떻게 해야하는데?
// A: 계산 인자는 메서드에 시그니처(인자)로 삽입한다.