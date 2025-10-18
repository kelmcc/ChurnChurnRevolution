using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemLogger : MonoBehaviour
{
    private void Update()
    {
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
        
        /*
        // Log mouse clicks
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Button: Left Click");
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Button: Right Click");
        }
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Mouse Button: Middle Click");
        }

        // Log mouse movement
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (mouseDelta != Vector2.zero)
        {
            Debug.Log($"Mouse Moved: {mouseDelta}");
        }

        // Log scroll wheel
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheel) > 0.01f)
        {
            Debug.Log($"Mouse Scroll: {scrollWheel}");
        }
    */
    }
}