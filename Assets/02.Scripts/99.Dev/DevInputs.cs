using UnityEngine;
using System.Collections.Generic;
using Gameplay.Board.Highlighter;
using Gameplay.Board.PathCalculator;

#if UNITY_EDITOR
public sealed class DevInputs : MonoBehaviour
{
    [SerializeField] private DevReset reset;
    //디버그용으로 남겨 놓음. 입력 스크립트로 옮길 때 필요할 수도 있음.
    [SerializeField] private BoardPathHighlighter highlighter;
    [SerializeField] private DevHintPipeline hintPipeline;
    [SerializeField] private DevReachablePipeline reachablePipeline;

    void Awake()
    {
        if (reset == null) reset = FindObjectOfType<DevReset>();
        if (highlighter == null) highlighter = FindObjectOfType<BoardPathHighlighter>();
        if (hintPipeline == null) hintPipeline = FindObjectOfType<DevHintPipeline>();
        if (reachablePipeline == null) reachablePipeline = FindObjectOfType<DevReachablePipeline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reset == null) return;

        if (Input.GetKeyDown(KeyCode.F11))
            reset.SoftReset();
        if (Input.GetKeyDown(KeyCode.F12))
            reset.HardReset();

        if (Input.GetKeyDown(KeyCode.T))
            hintPipeline.ToggleHint();
        if (Input.GetKeyDown(KeyCode.Y))
            reachablePipeline.ToggleReachable();
    }
}
#endif