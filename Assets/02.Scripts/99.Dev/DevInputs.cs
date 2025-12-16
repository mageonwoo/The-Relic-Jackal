#if UNITY_EDITOR
using UnityEngine;

public sealed class DevInputs : MonoBehaviour
{
    [SerializeField] private DevReset reset;
    // Start is called before the first frame update
    void Awake()
    {
        if (reset == null) reset = FindObjectOfType<DevReset>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reset == null) return;

        if (Input.GetKeyDown(KeyCode.F11))
            reset.SoftReset();
        if (Input.GetKeyDown(KeyCode.F12))
            reset.HardReset();
    }
}
#endif