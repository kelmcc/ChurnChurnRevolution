using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private float requiredTime = 2f;
    [SerializeField] private int requiredKeyCount = 3;

    private readonly HashSet<KeyCode> _pressedKeys = new HashSet<KeyCode>();
    private float _timer;

    private static readonly KeyCode[] MovementKeys =
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
        KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow
    };

    private void Update()
    {
        foreach (var key in MovementKeys)
        {
            if (Input.GetKeyDown(key))
            {
                if (_pressedKeys.Count == 0)
                {
                    _timer = requiredTime;
                }

                _pressedKeys.Add(key);
            }
        }

        if (_pressedKeys.Count > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _pressedKeys.Clear();
            }
        }

        if (_pressedKeys.Count >= requiredKeyCount)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}