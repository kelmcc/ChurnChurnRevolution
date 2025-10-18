using UnityEngine;

public sealed class ExitOnEsc : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private KeyCode _exitKey = KeyCode.Escape;
    [SerializeField] private bool _enableInEditor = true;   // Stop Play Mode in Editor
    [SerializeField] private bool _logWhenQuitting = true;

    private static ExitOnEsc _instance;

    private void Awake()
    {
        // Simple singleton + persist
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_exitKey))
        {
            if (_logWhenQuitting)
            {
                Debug.Log("ExitOnEsc: Quit requested.");
            }
            Quit();
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        if (_enableInEditor)
        {
            // Editor-safe: just stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
#endif
    }
}