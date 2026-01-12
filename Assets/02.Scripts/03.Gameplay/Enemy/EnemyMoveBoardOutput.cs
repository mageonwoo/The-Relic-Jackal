using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gameplay.Enemy
{
    public readonly struct EnemyMoveBoardOutput
    {
        public readonly Vector2Int nextPos;
        public readonly int nextRouteIndex;
        public readonly EnemyBoardMode boardMode;
        public readonly EnemyBoardState boardState;
        public EnemyMoveBoardOutput(Vector2Int _nextPos, int _nextRouteIndex,
                                    EnemyBoardMode _boardMode, EnemyBoardState _boardState)
        {
            this.nextPos = _nextPos;
            this.nextRouteIndex = _nextRouteIndex;
            this.boardMode = _boardMode;
            this.boardState = _boardState;
        }
    }

    // [알아두면 좋은 개념]

    // [return tuple]
    // tuple은 기술적 지름길이다.
    // 결과가 임시적이고 구조체 만들기 귀찮을 때, 함수형 스타일로 테스트 코드를 짠다고 생각하자.
    //
    // [예시]
    //  (var nextPos, var nextIndex, var nextMode)
    //      = EnemyMoveBoardAI.Calculate(input);
    //
    // 이런 형태를 가진다. 간편하지만, 개인적인 입장에서 쓸일은 딱히 없어 보인다.
    // 타입 이름이 사라지고, 의미를 주석이나 변수명에 의존한다는 명확한 단점이 있다고 한다.
    // 람다식과 같은 맥락에서 번거로움을 피하기위해 쓰는 모양인데. 난 쓸일이 없어 보인다.
}