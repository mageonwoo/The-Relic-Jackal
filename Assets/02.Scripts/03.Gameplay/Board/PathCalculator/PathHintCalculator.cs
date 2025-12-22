using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Board.PathCalculator
{
    public static class PathHintCalculator
    {
        public static List<Vector2Int> CalculateHintPath(BoardManager board, List<Vector2Int> fullPath, MoveCostBudget budget)
        {
            var result = new List<Vector2Int>();

            if (board == null || board.grid == null)
            {
                Debug.LogError("PathHintCalculator: BoardManager 또는 grid 누락");
                return result;
            }
            if (fullPath == null || fullPath.Count == 0)
            {
                Debug.LogWarning("PathHintCalculator: fullPath 누락");
                return result;
            }
            if (budget == null)
            {
                Debug.LogError("PathHintCalculator: MoveCostBudget 누락");
                return result;
            }

            int remainCost = budget.CurrentCost;
            if (remainCost <= 0) return result;

            for (int i = 0; i < fullPath.Count; i++)
            {
                Vector2Int pathTile = fullPath[i];

                if (!board.InBounds(pathTile))
                {
                    Debug.LogWarning("CalculateHintPath: 보드 범위 벗어남");
                    break;
                }

                int calculatingCost = board.grid[pathTile.x, pathTile.y].cost;
                if (remainCost - calculatingCost < 0) break;

                remainCost -= calculatingCost;
                result.Add(pathTile);
            }

            return result;
        }

        public static List<Vector2Int> CalculateHintPath(BoardManager board, List<Vector2Int> fullPath, int hintCost)
        {
            var result = new List<Vector2Int>();

            if (board == null || board.grid == null)
            {
                Debug.LogError("PathHintCalculator: BoardManager 또는 grid 누락");
                return result;
            }
            if (fullPath == null || fullPath.Count == 0)
            {
                Debug.LogWarning("PathHintCalculator: fullPath 누락");
                return result;
            }

            int remainCost = hintCost;
            if (remainCost <= 0)
                return result;

            for (int i = 0; i < fullPath.Count; i++)
            {
                Vector2Int p = fullPath[i];

                if (!board.InBounds(p))
                    break;

                int stepCost = board.grid[p.x, p.y].cost;
                if (remainCost - stepCost < 0)
                    break;

                remainCost -= stepCost;
                result.Add(p);
            }

            return result;
        }

    }
}