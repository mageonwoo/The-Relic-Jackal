using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardManager : MonoBehaviour
{
    public Tile[,] grid;
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public BoardMapData boardMapData;
    public int width = 8;
    public int height = 8;

    // Start is called before the first frame update
    void Awake()
    {
        GenerateGrid();
        ApplyMapObjects();
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

    void ApplyMapObjects()
    {
        foreach (var pos in boardMapData.obstacles)
        {
            grid[pos.x, pos.y].walkable = false;
            Instantiate(obstaclePrefab, grid[pos.x, pos.y].worldPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
