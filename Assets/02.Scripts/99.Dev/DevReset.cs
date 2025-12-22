using UnityEngine;
using UnityEngine.SceneManagement;
using Gameplay.Board.Highlighter;

#if UNITY_EDITOR

public sealed class DevReset : MonoBehaviour
{
    [SerializeField] BoardManager board;
    [SerializeField] PlayerCtrl player;
    [SerializeField] BoardPathHighlighter highlighter;

    void Awake()
    {
        if (board == null) board = FindObjectOfType<BoardManager>();
        if (player == null) player = FindObjectOfType<PlayerCtrl>();
        if (highlighter == null) highlighter = FindObjectOfType<BoardPathHighlighter>();
    }
    // Start is called before the first frame update
    public void SoftReset()
    {
        ClearHighlights();
        ResetPlayerSpawn();
    }

    public void ClearHighlights()
    {
        if (highlighter == null) return;

        foreach (HLLayer layer in System.Enum.GetValues(typeof(HLLayer)))
        {
            highlighter.Clear(layer);
        }
    }

    public void ResetPlayerSpawn()
    {
        if (board == null || board.boardMapData == null || player == null) return;
        player.StopFollowPath();

        var spawn = board.boardMapData.playerSpawn;
        player.SetGridPos(spawn.x, spawn.y);
    }

    // Update is called once per frame
    public void HardReset()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
#endif