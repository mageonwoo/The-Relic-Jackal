using System.Collections.Generic;
using UnityEngine;
using Gameplay.Board.Highlighter;
using Gameplay.Board.PathCalculator;

#if UNITY_EDITOR
public sealed class DevReachablePipeline : MonoBehaviour
{
    [SerializeField] private BoardManager board;
    [SerializeField] private PlayerCtrl player;
    [SerializeField] private BoardPathHighlighter highlighter;

    private bool showReachable = false;

    void Awake()
    {
        if (board == null) board = FindObjectOfType<BoardManager>();
        if (player == null) player = FindObjectOfType<PlayerCtrl>();
        if (highlighter == null) highlighter = FindObjectOfType<BoardPathHighlighter>();
    }

    public void ToggleReachable()
    {
        if (!showReachable) ShowReachable();
        else HideReachable();
    }

    public void ShowReachable()
    {
        if (showReachable) return;

        if (board == null || board.grid == null) return;
        if (player == null) return;
        if (highlighter == null) return;
        if (player.moveBudget == null) return;

        var start = new Vector2Int(player.x, player.y);

        // Reachable 계산
        List<Vector2Int> reachableCells =
        PathReachableCalculator.CalculateReachableCells(board, start, player.moveBudget);

        // GPT: reachable 검사 먼저 해서 NRE방지
        // reachableCells가 null 반환될 수 있어 사전 방어 
        if (reachableCells == null || reachableCells.Count == 0)
        {
            highlighter.Clear(HLLayer.Reachable);
            highlighter.Clear(HLLayer.Blocked);
            showReachable = false;
            return;
        }

        // reachable 기준으로 blocked 필터링
        List<Vector2Int> blockedCells = null;
        if (board.boardMapData != null && board.boardMapData.obstacles != null)
        {
            var reachableSet = new HashSet<Vector2Int>(reachableCells);
            blockedCells = new List<Vector2Int>();

            var obstacles = board.boardMapData.obstacles;

            // 4방향 인접 체크 (Dir4가 있으면 그걸 쓰고, 없으면 아래 배열 그대로)
            Vector2Int[] dir4 =
            {
                new Vector2Int( 1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int( 0, 1),
                new Vector2Int( 0,-1),
            };

            for (int i = 0; i < obstacles.Count; i++)
            {
                var o = obstacles[i];
                if (!board.InBounds(o)) continue;

                bool adjacentToReachable = false;
                for (int d = 0; d < dir4.Length; d++)
                {
                    var n = o + dir4[d];
                    if (reachableSet.Contains(n))
                    {
                        adjacentToReachable = true;
                        break;
                    }
                }

                if (adjacentToReachable)
                    blockedCells.Add(o);
            }
        }
        // blocked 먼저 그림
        if (blockedCells != null && blockedCells.Count > 0)
            highlighter.ShowCells(board, blockedCells, HLLayer.Blocked);

        highlighter.ShowCells(board, reachableCells, HLLayer.Reachable);
        showReachable = true;
    }

    public void HideReachable()
    {
        if (!showReachable) return;

        highlighter.Clear(HLLayer.Reachable);
        highlighter.Clear(HLLayer.Blocked);
        showReachable = false;
    }
}
#endif
