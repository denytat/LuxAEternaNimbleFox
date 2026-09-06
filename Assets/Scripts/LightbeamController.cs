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
    public SpriteRenderer beamSpriteRenderer;
    public Collider2D beamCollider;
    
    [Range(0f, 1f)]
    public float dimmedAlpha = 0.1f;

    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float drainRate = 1f;     // Drains fully in 5s
    public float rechargeRate = 15f;   // Recharges fully in ~6.6s
    public float minimumChargeToFire = 15f; // Must reach 15% to fire again after hitting 0%
    public float currentBattery { get; private set; }

    private bool _isOverheated = false; // Prevents 0% -> 1% -> 0% flickering

    [Header("Damage Settings")]
    public float damagePerSecond = 50f;

    public static bool IsBeamActive { get; private set; } = false;

    private ContactFilter2D _contactFilter;
    private List<Collider2D> _hitColliders = new List<Collider2D>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentBattery = maxBattery;

        _contactFilter = new ContactFilter2D();
        _contactFilter.NoFilter();
        _contactFilter.useTriggers = true;
    }

    void Update()
    {
        HandleBatteryAndInput();
        HandleBeamRotation();

        if (IsBeamActive && beamCollider != null)
        {
            ProcessLightImpacts();
        }
    }

    private void HandleBatteryAndInput()
    {
        // 1. Check Input
        bool wPressedLegacy = Input.GetKey(beamKey);
        bool wPressedNewInput = Keyboard.current != null && Keyboard.current.wKey.isPressed;
        bool wantsToFire = wPressedLegacy || wPressedNewInput;

        // 2. Overheat / Lockout State
        if (currentBattery <= 0f)
        {
            _isOverheated = true;
        }
        else if (_isOverheated && currentBattery >= minimumChargeToFire)
        {
            _isOverheated = false;
        }

        // 3. Process Drain vs Recharge
        if (wantsToFire)
        {
            if (!_isOverheated && currentBattery > 0f)
            {
                // Active firing: Drain battery
                IsBeamActive = true;
                currentBattery -= drainRate * Time.deltaTime;
                currentBattery = Mathf.Max(0f, currentBattery);
            }
            else
            {
                // Holding W while overheated/empty: Stay at 0% and DO NOT recharge
                IsBeamActive = false;
            }
        }
        else
        {
            // Releasing W: Allow battery to recharge back up
            IsBeamActive = false;
            if (currentBattery < maxBattery)
            {
                currentBattery += rechargeRate * Time.deltaTime;
                currentBattery = Mathf.Min(maxBattery, currentBattery);
            }
        }

        // 4. Visual Opacity (Stays dimmed when not actively firing)
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

            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ProcessLightExposure(damagePerSecond * Time.deltaTime);
                continue;
            }
        }
    }
}