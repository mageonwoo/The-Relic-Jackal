using System.Collections.Generic;
using UnityEngine;
using Gameplay.Board.Highlighter;
using Gameplay.Board.PathCalculator;
#if UNITY_EDITOR

public class DevHintPipeline : MonoBehaviour
{
    [SerializeField] private BoardManager board;
    [SerializeField] private PlayerCtrl player;
    [SerializeField] private BoardPathHighlighter highlighter;
    [SerializeField] private int hintLength = 10;
    bool showHintPath = false;

    void Awake()
    {
        if (board == null) board = FindObjectOfType<BoardManager>();
        if (player == null) player = FindObjectOfType<PlayerCtrl>();
        if (highlighter == null) highlighter = FindObjectOfType<BoardPathHighlighter>();
    }

    public void ToggleHint()
    {
        if (!showHintPath)
            ShowHint();
        else
            HideHint();
    }
    public void ShowHint()
    {
        if (showHintPath) return;

        if (board == null || board.grid == null) return;
        if (player == null) return;
        if (highlighter == null) return;
        if (board.boardMapData == null) return;

        Vector2Int goal = board.boardMapData.GoalPoint;

        // HintPathCalculator는 전체경로에서 힌트경로를 계산할 뿐, 경로를 직접 만들지는 못함.
        // AstarPathFinder는 경로를 만드는 역할을 하지만, 저장하거나 유지하지 않음.
        // 따라서 힌트경로를 출력하기 위해선 전체경로를 외부에서 준비해 줘야함. 이 준비하는 Method가 ShowHint 자신이 된다.
        // 이때의 전체 경로는 ShowHint 호출 시점의 플레이어 현재 좌표를 기준으로 계산된다. 오래된 경로를 계산하게 하면 안 된다.
        // 외부에서 준비한 전체경로와 코스트자산을 근거로 HintPathCalculator에서 힌트 경로를 계산해
        // Highlighter.ShowCells를 통해 시각적으로 출력하는 파이프라인을 거친다.
        List<Vector2Int> fullPath =
            AstarPathfinder.FindPath(board, player.x, player.y, goal.x, goal.y);
        if (fullPath == null || fullPath.Count == 0)
        {
            Debug.Log("ShowHint: fullPath 없음");
            return;
        }

        List<Vector2Int> hintPath =
            PathHintCalculator.CalculateHintPath(board, fullPath, hintLength);
        if (hintPath == null || hintPath.Count == 0)
        {
            highlighter.Clear(HLLayer.Hint);
            showHintPath = false;
            return;
        }

        highlighter.ShowCells(board, hintPath, HLLayer.Hint);
        showHintPath = true;
    }
    public void HideHint()
    {
        if (!showHintPath) return;

        highlighter.Clear(HLLayer.Hint);
        showHintPath = false;
    }
}
#endif