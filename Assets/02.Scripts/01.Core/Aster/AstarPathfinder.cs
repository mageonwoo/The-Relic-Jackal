using System.Collections.Generic;
using UnityEngine;

public static class AstarPathfinder
{
    private class Node
    {
        public int x;
        public int y;

        public int g;
        public int h;
        public int f => g + h;

        public Node parent;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static readonly (int dx, int dy)[] Dir4 =
    {
        (0,1),(0,-1),(-1,0),(1,0)
    };

    private static int Heuristic(int x, int y, int goalX, int goalY)
    {
        return Mathf.Abs(x - goalX) + Mathf.Abs(y - goalY);
    }

    private static int Heuristic(Vector2Int curPos, Vector2Int goalPos)
    {
        return Mathf.Abs(curPos.x - goalPos.x) + Mathf.Abs(curPos.y - goalPos.y);
    }

    public static List<Vector2Int> FindPath(BoardManager board, int startX, int startY, int goalX, int goalY)
    {
        if (board == null || board.grid == null) return null;
        //InBounds = 범위 체크 매서드
        //start와 goal의 그리드 좌표가 배열을 벗어나면 null을 리턴한다.
        if (!InBounds(startX, startY, board.width, board.height)) return null;
        if (!InBounds(goalX, goalY, board.width, board.height)) return null;

        //start와 goal의 그리드 좌표가 walkable이 아니면 null을 리턴한다.
        if (board.IsWalkable(new Vector2Int(startX, startY)) == false) return null;
        if (board.IsWalkable(new Vector2Int(goalX, goalY)) == false) return null;

        /// open : “누굴 다음에 볼까?” (우선순위)
        /// openMap : “이 좌표 이미 봤어?” (존재 확인)
        /// closed : “이 좌표는 끝났어” (재탐색 방지)

        //발견되었지만 아직 확정되지 않은 Node를 open에 저장한다.
        var open = new List<Node>();
        //같은 좌표가 open에 이미 있는지 빠르게 확인하기 위해 openMap에 좌표,Node를 함께 저장한다.
        var openMap = new Dictionary<Vector2Int, Node>();
        //이미 확정된 좌표는 closed에 넣어 다시 탐색하지 않게 한다.
        var closed = new HashSet<Vector2Int>();

        var start = new Node(startX, startY);
        start.g = 0;
        start.h = Heuristic(startX, startY, goalX, goalY);

        open.Add(start);
        openMap[new Vector2Int(startX, startY)] = start;

        while (open.Count > 0)
        {
            Node current = PopLowestF(open);

            Vector2Int curPos = new Vector2Int(current.x, current.y);
            openMap.Remove(curPos);
            closed.Add(curPos);

            if (current.x == goalX && current.y == goalY)
            {
                return BuildPath(current, startX, startY);
            }

            for (int i = 0; i < Dir4.Length; i++)
            {
                int nx = current.x + Dir4[i].dx;
                int ny = current.y + Dir4[i].dy;

                if (!board.InBounds(new Vector2Int(nx, ny))) continue;
                if (!board.IsWalkable(new Vector2Int(nx, ny))) continue;

                Vector2Int nPos = new Vector2Int(nx, ny);
                if (closed.Contains(nPos)) continue;

                //cost 사용
                int stepCost = board.GetCost(nPos);
                int tentativeG = current.g + stepCost;

                if (openMap.TryGetValue(nPos, out Node exist))
                {
                    if (tentativeG < exist.g)
                    {
                        exist.g = tentativeG;
                        exist.parent = current;
                    }
                }
                else
                {
                    Node node = new Node(nx, ny);
                    node.g = tentativeG;
                    node.h = Heuristic(nx, ny, goalX, goalY);
                    node.parent = current;

                    open.Add(node);
                    openMap[nPos] = node;
                }
            }
        }
        return null;
    }

    public static List<Vector2Int> FindPath(BoardManager board, Vector2Int start, Vector2Int goal)
    {
        if (board == null || board.grid == null) return null;
        //InBounds = 범위 체크 매서드
        //start와 goal의 그리드 좌표가 배열을 벗어나면 null을 리턴한다.
        if (!board.InBounds(start)) return null;
        if (!board.InBounds(goal)) return null;

        //start와 goal의 그리드 좌표가 walkable이 아니면 null을 리턴한다.
        if (!board.IsWalkable(start)) return null;
        if (!board.IsWalkable(goal)) return null;

        /// open : “누굴 다음에 볼까?” (우선순위)
        /// openMap : “이 좌표 이미 봤어?” (존재 확인)
        /// closed : “이 좌표는 끝났어” (재탐색 방지)

        //발견되었지만 아직 확정되지 않은 Node를 open에 저장한다.
        var open = new List<Node>();
        //같은 좌표가 open에 이미 있는지 빠르게 확인하기 위해 openMap에 좌표,Node를 함께 저장한다.
        var openMap = new Dictionary<Vector2Int, Node>();
        //이미 확정된 좌표는 closed에 넣어 다시 탐색하지 않게 한다.
        var closed = new HashSet<Vector2Int>();

        var startNode = new Node(start.x, start.y);
        startNode.g = 0;
        startNode.h = Heuristic(start, goal);

        open.Add(startNode);
        openMap[start] = startNode;

        while (open.Count > 0)
        {
            Node current = PopLowestF(open);

            Vector2Int curPos = new Vector2Int(current.x, current.y);
            openMap.Remove(curPos);
            closed.Add(curPos);

            if (current.x == goal.x && current.y == goal.y)
            {
                return BuildPath(current, start.x, start.y);
            }

            for (int i = 0; i < Dir4.Length; i++)
            {
                int nx = current.x + Dir4[i].dx;
                int ny = current.y + Dir4[i].dy;

                if (!board.InBounds(new Vector2Int(nx, ny))) continue;
                if (!board.IsWalkable(new Vector2Int(nx, ny))) continue;

                Vector2Int nPos = new Vector2Int(nx, ny);
                if (closed.Contains(nPos)) continue;

                //cost 사용
                int stepCost = board.GetCost(nPos);
                int tentativeG = current.g + stepCost;

                if (openMap.TryGetValue(nPos, out Node exist))
                {
                    if (tentativeG < exist.g)
                    {
                        exist.g = tentativeG;
                        exist.parent = current;
                    }
                }
                else
                {
                    Node node = new Node(nx, ny);
                    node.g = tentativeG;
                    node.h = Heuristic(start, goal);
                    node.parent = current;

                    open.Add(node);
                    openMap[nPos] = node;
                }
            }
        }
        return null;
    }

    //범위 체크
    public static bool InBounds(int x, int y, int width, int height)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    //open 리스트에서 f 최소 노드 꺼내기
    private static Node PopLowestF(List<Node> open)
    {
        int bestIndex = 0;
        Node best = open[0];

        for (int i = 1; i < open.Count; i++)
        {
            Node n = open[i];

            if (n.f < best.f || (n.f == best.f && n.h < best.h))
            {
                best = n;
                bestIndex = i;
            }
        }

        open.RemoveAt(bestIndex);
        return best;
    }

    //parent를 타고 거꾸로 올라가서 경로 구성
    private static List<Vector2Int> BuildPath(Node goalNode, int startX, int startY)
    {
        var path = new List<Vector2Int>();

        Node cur = goalNode;
        while (cur != null)
        {
            if (!(cur.x == startX && cur.y == startY))
                path.Add(new Vector2Int(cur.x, cur.y));

            cur = cur.parent;
        }

        path.Reverse();
        return path;
    }
}
