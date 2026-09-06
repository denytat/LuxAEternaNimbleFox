using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LightbeamController : MonoBehaviour
{
    public static LightbeamController Instance { get; private set; }

    [Header("Rotation Settings")]
    public float rotationSpeed = 250f;

    [Header("Beam Power Settings")]
    public KeyCode beamKey = KeyCode.W;
    public SpriteRenderer beamSpriteRenderer; // Drag 'LightBeam' child sprite here
    public Collider2D beamCollider;           // Drag 'LightBeam' child collider here
    
    [Range(0f, 1f)]
    public float dimmedAlpha = 0.1f; // Opacity when W is not held

    [Header("Damage & Purification Settings")]
    public float damagePerSecond = 50f;

    public static bool IsBeamActive { get; private set; } = false;

    private ContactFilter2D _contactFilter;
    private List<Collider2D> _hitColliders = new List<Collider2D>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Setup filter to catch all triggers and physics colliders
        _contactFilter = new ContactFilter2D();
        _contactFilter.NoFilter();
        _contactFilter.useTriggers = true;
    }

    void Update()
    {
        HandleBeamPower();
        HandleBeamRotation();

        // Perform continuous light overlap processing when W is active
        if (IsBeamActive && beamCollider != null)
        {
            ProcessLightImpacts();
        }
    }

    private void HandleBeamPower()
    {
        // Support Legacy & New Input System
        bool wPressedLegacy = Input.GetKey(beamKey);
        bool wPressedNewInput = Keyboard.current != null && Keyboard.current.wKey.isPressed;

        IsBeamActive = wPressedLegacy || wPressedNewInput;

        // Visual Feedback: Active (1.0 Alpha) vs Dimmed (0.1 Alpha)
        if (beamSpriteRenderer != null)
        {
            Color c = beamSpriteRenderer.color;
            c.a = IsBeamActive ? 1.0f : dimmedAlpha;
            beamSpriteRenderer.color = c;
        }
    }

    private void HandleBeamRotation()
    {
        float dir = 0f;
        if (Input.GetKey(KeyCode.Q) || (Keyboard.current != null && Keyboard.current.qKey.isPressed)) dir += 1f;
        if (Input.GetKey(KeyCode.E) || (Keyboard.current != null && Keyboard.current.eKey.isPressed)) dir -= 1f;

        if (dir != 0f)
        {
            transform.Rotate(0f, 0f, dir * rotationSpeed * Time.deltaTime);
        }
    }

    private void ProcessLightImpacts()
    {
        _hitColliders.Clear();
        int hitCount = beamCollider.Overlap(_contactFilter, _hitColliders);

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D col = _hitColliders[i];
            if (col == null || col.gameObject == gameObject) continue;

            // 1. Process Standard Enemies (Handles active damage & dead corpse conversion)
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ProcessLightExposure(damagePerSecond * Time.deltaTime);
                continue;
            }
        }
    }
}