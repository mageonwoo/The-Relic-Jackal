using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//순수 데이터 클래스이기 때문에 Core로 분류한다. 
public class Tile
{
    public int x;
    public int y;
    public bool walkable = true;
    public int cost = 1;
    public Vector3 worldPos;
}
