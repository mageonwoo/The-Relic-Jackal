#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class DevReset : MonoBehaviour
{
    [SerializeField] BoardManager board;
    [SerializeField] PlayerCtrl player;
    //[SerializeField] PathHighlighter highlighter;

    void Awake()
    {
        if (board == null) board = FindObjectOfType<BoardManager>();
        if (player == null) player = FindObjectOfType<PlayerCtrl>();
        // if (highlighter == null) highlighter = FindObjectOfType<PathHighlighter>();
    }
    // Start is called before the first frame update
    public void SoftReset()
    {
        ClearHighlight();
        ResetPlayerSpawn();
    }

    public void ClearHighlight()
    {
        // if (highlighter != null)
        //     highlighter.Clear();
    }

    public void ResetPlayerSpawn()
    {
        if (board == null || board.boardMapData == null||player==null) return;
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