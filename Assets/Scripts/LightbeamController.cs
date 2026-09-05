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
    public Collider2D beamCollider; // Drag your LightBeam's Collider2D here!
    
    [Range(0f, 1f)]
    public float dimmedAlpha = 0.1f;

    [Header("Damage Settings")]
    public float damagePerSecond = 50f;

    public static bool IsBeamActive { get; private set; } = false;

    // Contact filter to catch trigger colliders
    private ContactFilter2D _contactFilter;
    private List<Collider2D> _hitColliders = new List<Collider2D>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Setup filter to detect all triggers and colliders
        _contactFilter = new ContactFilter2D();
        _contactFilter.NoFilter();
        _contactFilter.useTriggers = true;
    }

    void Update()
    {
        // 1. Check Input
        bool wPressedLegacy = Input.GetKey(beamKey);
        bool wPressedNewInput = Keyboard.current != null && Keyboard.current.wKey.isPressed;
        IsBeamActive = wPressedLegacy || wPressedNewInput;

        // 2. Visual Opacity Shift
        if (beamSpriteRenderer != null)
        {
            Color c = beamSpriteRenderer.color;
            c.a = IsBeamActive ? 1.0f : dimmedAlpha;
            beamSpriteRenderer.color = c;
        }

        // 3. Q/E Rotation
        float dir = 0f;
        if (Input.GetKey(KeyCode.Q) || (Keyboard.current != null && Keyboard.current.qKey.isPressed)) dir += 1f;
        if (Input.GetKey(KeyCode.E) || (Keyboard.current != null && Keyboard.current.eKey.isPressed)) dir -= 1f;

        if (dir != 0f)
        {
            transform.Rotate(0f, 0f, dir * rotationSpeed * Time.deltaTime);
        }

        // 4. DIRECT OVERLAP DAMAGE CHECK (Bypasses Unity Trigger events)
        if (IsBeamActive && beamCollider != null)
        {
            ApplyBeamDamage();
        }
    }

    private void ApplyBeamDamage()
    {
        _hitColliders.Clear();
        int hitCount = beamCollider.Overlap(_contactFilter, _hitColliders);

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D col = _hitColliders[i];
            if (col == null || col.gameObject == gameObject) continue;

            // Damage Normal Enemy
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
                Debug.Log($"[BEAM OVERLAP] Damaging Enemy: {col.name}");
                continue;
            }

            // Damage Boss Enemy
            BossEnemy boss = col.GetComponent<BossEnemy>();
            if (boss != null)
            {
                boss.TakeDamage(damagePerSecond * Time.deltaTime);
                Debug.Log($"[BEAM OVERLAP] Damaging Boss: {col.name}");
            }
        }
    }
}