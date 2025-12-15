using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public BoardManager board;
    public int x;
    public int y;

    public float stepDelay = 0.5f;
    private List<Vector2Int> _testpath;
    private Coroutine _followCor;

    // Update is called once per frame
    void Start()
    {
        if (board == null)
        {
            Debug.Log("BoardManager is gone. Getting Process to find BoardManager...");
            board = FindObjectOfType<BoardManager>();
        }

        if (board != null && board.boardMapData != null)
        {
            x = board.boardMapData.playerSpawn.x;
            y = board.boardMapData.playerSpawn.y;
        }

        transform.position = board.grid[x, y].worldPos;
    }
    void Update()
    {
        if (_followCor == null)
        {
            if (Input.GetKeyDown(KeyCode.W))
                TryMove(0, 1);
            if (Input.GetKeyDown(KeyCode.S))
                TryMove(0, -1);
            if (Input.GetKeyDown(KeyCode.A))
                TryMove(-1, 0);
            if (Input.GetKeyDown(KeyCode.D))
                TryMove(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.P))
            TestPath();
    }

    void TryMove(int dx, int dy)
    {
        int targetX = x + dx;
        int targetY = y + dy;

        if (targetX < 0 || targetY < 0 || targetX >= board.width || targetY >= board.height)
            return;

        if (board.grid[targetX, targetY].walkable == false)
        {
            //애니메이션 트리거
            return;
        }

        x = targetX;
        y = targetY;
        transform.position = board.grid[x, y].worldPos;
    }

    void TestPath()
    {
        if (board == null || board.grid == null)
        {
            Debug.LogError("TestPath(): board/board.grid 누락");
            return;
        }

        int goalX = board.width - 1;
        int goalY = board.height - 1;

        var path = AstarPathfinder.FindPath(board, x, y, goalX, goalY);

        if (path == null)
        {
            Debug.Log("TestPath().path값 없음");
            return;
        }

        _testpath = path;

        if (_followCor != null) StopCoroutine(_followCor);
        _followCor = StartCoroutine(CoFollowPath(_testpath));
    }

    IEnumerator CoFollowPath(List<Vector2Int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int next = path[i];

            int dx = next.x - x;
            int dy = next.y - y;

            if (Mathf.Abs(dx) + Mathf.Abs(dy) != 1)
            {
                Debug.LogError("4방향 규칙 위반: 허용되지 않은 스텝");
                break;
            }

            int beforeX = x;
            int beforeY = y;

            TryMove(dx, dy);

            if (x == beforeX && y == beforeY)
            {
                Debug.Log("CoFollowPath: blocked, stop");
                break;
            }

            yield return new WaitForSeconds(stepDelay);
        }
        _followCor = null;
    }
}
