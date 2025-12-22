using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardManager : MonoBehaviour
{
    public Tile[,] grid;
    public GameObject tilePrefab; GameObject[,] _tileInstances;
    public GameObject obstaclePrefab; GameObject[,] _obstacleInstances;
    public GameObject goalPrefab; GameObject _goalInstance;
    public BoardMapData boardMapData;
    public int width;
    public int height;

    // Start is called before the first frame update
    void Awake()
    {
        GenerateGrid();
        ApplyMapObstacles();
        ApplyGoal();
    }

    public bool InBounds(Vector2Int p)
    {
        if (grid == null)
            return false;

        return p.x >= 0 && p.y >= 0 &&
                p.x < grid.GetLength(0) &&
                p.y < grid.GetLength(1);
    }

    void GenerateGrid()
    {
        grid = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = new Tile();
                tile.x = x;
                tile.y = y;
                tile.walkable = true;

                Vector3 pos = new Vector3(x, 0, y);
                tile.worldPos = pos;

                grid[x, y] = tile;

                Instantiate(tilePrefab, pos, Quaternion.identity);
            }
        }
    }

    void ApplyMapObstacles()
    {
        foreach (var pos in boardMapData.obstacles)
        {
            grid[pos.x, pos.y].walkable = false;
            Instantiate(obstaclePrefab, grid[pos.x, pos.y].worldPos, Quaternion.identity);
        }
    }

    //콘텐츠 개발 중 좌표 실수로 골 지점이 장애물 위에 생겨 논리/시각 불일치를 방지하기 위해 입력 제외
    //하나의 보드에 하나만 존재해야 하므로 중복방지 로직 작성
    void ApplyGoal()
    {
        var pos = boardMapData.GoalPoint;

        if (_goalInstance != null)
            Destroy(_goalInstance);

        _goalInstance = Instantiate(goalPrefab, grid[pos.x, pos.y].worldPos, Quaternion.identity);
    }
}
