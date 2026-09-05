using UnityEngine;
using UnityEngine.InputSystem;

public class LightbeamController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 250f;

    [Header("Beam Power Settings")]
    public KeyCode beamKey = KeyCode.W;
    public SpriteRenderer beamSpriteRenderer; 
    
    [Range(0f, 1f)]
    public float dimmedAlpha = 0.1f;
    
    public static bool IsBeamActive { get; private set; } = false;

    private Color _fullColor;
    private Color _dimmedColor;

    void Start()
    {
        if (beamSpriteRenderer != null)
        {
            _fullColor = beamSpriteRenderer.color;
            _fullColor.a = 1f;

            _dimmedColor = _fullColor;
            _dimmedColor.a = dimmedAlpha;
        }
    }

    void Update()
    {
        bool wPressedLegacy = Input.GetKey(beamKey);
        bool wPressedNewInput = Keyboard.current != null && Keyboard.current.wKey.isPressed;

        IsBeamActive = wPressedLegacy || wPressedNewInput;

        if (beamSpriteRenderer != null)
        {
            beamSpriteRenderer.color = IsBeamActive ? _fullColor : _dimmedColor;
        }

        float dir = 0f;

        if (Input.GetKey(KeyCode.E) || (Keyboard.current != null && Keyboard.current.eKey.isPressed)) dir -= 1f;
        if (Input.GetKey(KeyCode.Q) || (Keyboard.current != null && Keyboard.current.qKey.isPressed)) dir += 1f;

        if (dir != 0f)
        {
            transform.Rotate(0f, 0f, dir * rotationSpeed * Time.deltaTime);
        }
    }
}