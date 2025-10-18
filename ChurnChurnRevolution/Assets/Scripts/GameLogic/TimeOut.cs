using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TimeOut : MonoBehaviour
{
    [SerializeField] private float inputWindow = 2f;
    [SerializeField] private int requiredKeyCount = 3;
    [SerializeField] private float timeoutDuration = 30f;

    private float _timeoutTimer;
    private float _inputWindowTimer;

    private readonly HashSet<KeyCode> _pressedKeys = new HashSet<KeyCode>();
    private readonly HashSet<string> _joystickDirections = new HashSet<string>();

    private static readonly KeyCode[] MovementKeys =
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
        KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow
    };

    private const float JoystickThreshold = 0.5f;

    private void Start()
    {
        _timeoutTimer = timeoutDuration;
        _inputWindowTimer = inputWindow;
    }

    private void Update()
    {
        bool newInputDetected = false;

        foreach (var key in MovementKeys)
        {
            if (Input.GetKeyDown(key))
            {
                if (_pressedKeys.Count == 0 && _joystickDirections.Count == 0)
                {
                    _inputWindowTimer = inputWindow;
                }

                if (_pressedKeys.Add(key))
                {
                    newInputDetected = true;
                }
            }
        }

        foreach (var joy in Joystick.all)
        {
            Vector2 stick = joy.stick.ReadValue();

            if (stick.x > JoystickThreshold)
            {
                if (_joystickDirections.Count == 0 && _pressedKeys.Count == 0)
                    _inputWindowTimer = inputWindow;

                if (_joystickDirections.Add($"{joy.deviceId}_Right"))
                    newInputDetected = true;
            }
            else if (stick.x < -JoystickThreshold)
            {
                if (_joystickDirections.Add($"{joy.deviceId}_Left"))
                    newInputDetected = true;
            }

            if (stick.y > JoystickThreshold)
            {
                if (_joystickDirections.Add($"{joy.deviceId}_Up"))
                    newInputDetected = true;
            }
            else if (stick.y < -JoystickThreshold)
            {
                if (_joystickDirections.Add($"{joy.deviceId}_Down"))
                    newInputDetected = true;
            }
        }

        // ==== Timer logic ====
        if (_pressedKeys.Count > 0 || _joystickDirections.Count > 0)
        {
            _inputWindowTimer -= Time.deltaTime;
            if (_inputWindowTimer <= 0f)
            {
                _pressedKeys.Clear();
                _joystickDirections.Clear();
            }
        }

        if (_pressedKeys.Count + _joystickDirections.Count >= requiredKeyCount)
        {
            _timeoutTimer = timeoutDuration;
            _pressedKeys.Clear();
            _joystickDirections.Clear();
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