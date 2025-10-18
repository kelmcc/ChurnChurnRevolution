using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemLogger : MonoBehaviour
{
    public bool DebugInput = false;

    private void Start()
    {
        if (!DebugInput)
        {
            return;
        }

        Debug.Log($"Input System active: {InputSystem.settings != null}");
        Debug.Log($"Joystick connected: {Joystick.all.Count}");

        foreach (var d in InputSystem.devices)
        {
            Debug.Log($"Device: {d.displayName}, Layout: {d.layout}, Type: {d.GetType().Name}");
        }
    }

    private void Update()
    {
        if (!DebugInput)
        {
            return;
        }

        // Log all keyboard input
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                Debug.Log($"Key Pressed: {keyCode}");
            }
        }

        for (int i = 0; i <= 19; i++)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0 + i))
            {
                Debug.Log($"Joystick Button {i} pressed");
            }
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            Debug.Log($"Axis Movement: H={horizontal} V={vertical}");
        }

        foreach (var joy in Joystick.all)
        {
            Vector2 pos = joy.stick.ReadValue();
            if (pos.magnitude > 0.1f)
            {
                Debug.Log($"{joy.displayName}{joy.deviceId}: {pos}");
            }
        }
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (!DebugInput)
        {
            return;
        }
        
        Debug.Log($"Device change: {device.displayName} - {change}");
    }
}