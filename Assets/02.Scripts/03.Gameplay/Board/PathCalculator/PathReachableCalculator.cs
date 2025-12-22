using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Board.PathCalculator
{
    public static class PathReachableCalculator
    {
        public static List<Vector2Int> CalculateReachableCells(BoardManager board, Vector2Int start, MoveCostBudget budget)
        {
            var result = new List<Vector2Int>();

            if (board == null || board.grid == null)
            {
                Debug.LogError("PathReachableCalculator: BoardManager 또는 grid 누락");
                return result;
            }
            if (!board.InBounds(start))
            {
                Debug.LogWarning("PathReachableCalculator: start 보드 범위 벗어남");
                return result;
            }
            if (budget == null)
            {
                Debug.LogError("PathReachableCalculator: MoveCostBudget 누락");
                return result;
            }

            int maxCost = budget.CurrentCost;
            if (maxCost <= 0) return result;

            if (board.grid[start.x, start.y].walkable == false) return result;

            // 보드의 각 좌표(Vector2Int)에 대해 그 좌표에 도달했을 때의 최소 누적 cost(int) 를 저장하는 컨테이너
            var bestCost = new Dictionary<Vector2Int, int>();

            // 시작 지점 비용
            // start는 이미 방문 + start의 최적비용은 0
            bestCost[start] = 0;

            // 앞으로 탐색할 좌표들
            // + 아직 처리하지 않았지만, 처리할 가치가 있는 좌표들
            // 1) walkable = true
            // 2) CurrentCost <= maxCost
            // 3) 이전에 더 좋은 비용으로 방문한 적 없는 타일

            // 앞으로 검사할 좌표와 그 좌표까지의 누적 비용을 한 쌍으로 보관하기 위한 컨테이너
            var open = new List<(int cost, Vector2Int pos)>();
            open.Add((0, start));

            while (open.Count > 0)
            {
                // open에서 하나 꺼내서 검사
                // 검사 단계로 들어간 좌표를 cur에 저장하고 리스트에서 제거
                // start는 확장 기준점으로써 반드시 검사한다.
                var cur = open[0];
                open.RemoveAt(0);

                int curCost = cur.cost;
                Vector2Int curPos = cur.pos;

                // GPT: 오래된 후보를 스킵하는 로직을 추가해라. 훨씬 안정적이다
                if (bestCost.TryGetValue(curPos, out int knownCost) && curCost != knownCost) continue;

                // start가 가진 좌표정보는 기준점. 지금 플레이어가 서있는 좌표. 포함 안됨.
                // while 반복문을 돌아서 여기에 도착하면 curPos가 start와 같은 좌표인지 확인 후 아니라면 리스트에 추가함.
                if (curPos != start)
                    result.Add(curPos);

                // 1. curPos기준으로 4방향 검사
                for (int d = 0; d < AstarPathfinder.Dir4.Length; d++)
                {
                    int nextX = curPos.x + AstarPathfinder.Dir4[d].dx;
                    int nextY = curPos.y + AstarPathfinder.Dir4[d].dy;

                    var nextPos = new Vector2Int(nextX, nextY);

                    // 1. 범위 밖 검색 방어
                    if (!board.InBounds(nextPos)) continue;

                    // 2. 장애물 검사
                    if (board.grid[nextX, nextY].walkable == false) continue;

                    // 3. 해당 좌표의 타일로 이동하는 데 사용되는 코스트를 확인하기 위한 변수
                    int movingCost = board.grid[nextX, nextY].cost;

                    // 4. maxCost와 비교해 초과여부를 검사할 누적비용 변수
                    // 검사에 사용한 curCost + 이동하는 데 사용되는 movingCost
                    int accumulatedCost = curCost + movingCost;

                    // 5. accumulatedCost가 maxCost를 초과하면 스킵
                    if (accumulatedCost > maxCost) continue;

                    // 6. nextPos를 더 싼 비용으로 방문한 적이 있으면 스킵
                    // 7. 방문한 적이 없으면, 갱신하고 다음 확장 후보로 등록
                    // - TryGetValue로 nextPos로 가는 비용을 산출해 이 비용이 지금 계산한 accumlatedCost보다 작다면 계산의 의미가 없다는 의미
                    if (bestCost.TryGetValue(nextPos, out int prevCost) && prevCost <= accumulatedCost) continue;
                    bestCost[nextPos] = accumulatedCost;
                    open.Add((accumulatedCost, nextPos));
                }
            }
            return result;
        }
    }
}

// V2: 타일 타입(슬로우/함정)에 따라 movingCost/추가 패널티 계산 확장