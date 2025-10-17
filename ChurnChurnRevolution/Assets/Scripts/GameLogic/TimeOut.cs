using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeOut : MonoBehaviour
{
    [SerializeField] private float inputWindow = 2f;
    [SerializeField] private int requiredKeyCount = 3;
    [SerializeField] private float timeoutDuration = 30f;

    private float _timeoutTimer;
    private float _inputWindowTimer;
    private readonly HashSet<KeyCode> _pressedKeys = new HashSet<KeyCode>();

    private static readonly KeyCode[] MovementKeys =
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
        KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow
    };

    private void Start()
    {
        _timeoutTimer = timeoutDuration;
        _inputWindowTimer = inputWindow;
    }

    private void Update()
    {
        foreach (var key in MovementKeys)
        {
            if (Input.GetKeyDown(key))
            {
                if (_pressedKeys.Count == 0)
                {
                    _inputWindowTimer = inputWindow;
                }

                _pressedKeys.Add(key);
            }
        }

        if (_pressedKeys.Count > 0)
        {
            _inputWindowTimer -= Time.deltaTime;
            if (_inputWindowTimer <= 0f)
            {
                _pressedKeys.Clear();
            }
        }

        if (_pressedKeys.Count >= requiredKeyCount)
        {
            _timeoutTimer = timeoutDuration;
            _pressedKeys.Clear();
        }
        else
        {
            _timeoutTimer -= Time.deltaTime;
        }

        if (_timeoutTimer <= 0f)
        {
            SceneManager.LoadScene(0);
        }
    }
}