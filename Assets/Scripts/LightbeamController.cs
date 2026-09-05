using UnityEngine;
using UnityEngine.InputSystem; // Required for the New Input System

public class LightbeamController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 250f;

    void Update()
    {
        if (Keyboard.current == null) return;

        float rotationInput = 0f;

        if (Keyboard.current.eKey.isPressed)
        {
            rotationInput -= 1f;
        }

        if (Keyboard.current.qKey.isPressed)
        {
            rotationInput += 1f;
        }

        if (rotationInput != 0f)
        {
            transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
        }
    }
}