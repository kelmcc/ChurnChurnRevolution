using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private float _requiredTime = 2f;
    [SerializeField] private int _requiredKeyCount = 3;
    [SerializeField] private Transitions _trans;

    private readonly HashSet<KeyCode> _pressedKeys = new HashSet<KeyCode>();
    private readonly HashSet<string> _joystickDirections = new HashSet<string>();

    private float _timer;

    private static readonly KeyCode[] MovementKeys =
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
        KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow
    };

    private const float JoystickThreshold = 0.5f;

    private void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        bool inputStarted = false;

        foreach (var key in MovementKeys)
        {
            if (Input.GetKeyDown(key))
            {
                if (_pressedKeys.Count == 0 && _joystickDirections.Count == 0)
                    _timer = _requiredTime;

                _pressedKeys.Add(key);
            }
        }

        foreach (var joy in Joystick.all)
        {
            Vector2 stick = joy.stick.ReadValue();

            if (stick.x > JoystickThreshold)
            {
                if (_pressedKeys.Count == 0 && _joystickDirections.Count == 0)
                    _timer = _requiredTime;

                _joystickDirections.Add($"{joy.deviceId}_Right");
            }
            else if (stick.x < -JoystickThreshold)
            {
                _joystickDirections.Add($"{joy.deviceId}_Left");
            }

            if (stick.y > JoystickThreshold)
            {
                _joystickDirections.Add($"{joy.deviceId}_Up");
            }
            else if (stick.y < -JoystickThreshold)
            {
                _joystickDirections.Add($"{joy.deviceId}_Down");
            }
        }

        if (_pressedKeys.Count > 0 || _joystickDirections.Count > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _pressedKeys.Clear();
                _joystickDirections.Clear();
            }
        }

        if (_pressedKeys.Count + _joystickDirections.Count >= _requiredKeyCount)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}