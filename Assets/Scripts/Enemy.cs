using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float moveSpeed = 2.5f;
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float beamDamagePerSecond = 50f; // Directly control damage here

    [Header("Death Visuals")]
    public Sprite deadSprite;

    private Transform _coreTransform;
    private float _currentHealth;
    private bool _isDead = false;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    void Start()
    {
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

        GameObject coreObj = GameObject.FindGameObjectWithTag("Core");
        if (coreObj != null) _coreTransform = coreObj.transform;
    }

    void Update()
    {
        if (_isDead) return;

        // Move toward core
        if (_coreTransform != null)
        {
            Vector2 direction = (_coreTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isDead) return;

        // Check if touching LightBeam AND beam is powered up with W
        if (other.CompareTag("LightBeam"))
        {
            if (LightbeamController.IsBeamActive)
            {
                TakeDamage(beamDamagePerSecond * Time.deltaTime);
                Debug.Log($"[DAMAGE APPLIED] Enemy HP: {_currentHealth}/{maxHealth}");
            }
            else
            {
                Debug.Log("[TOUCHING BEAM] Hold W to activate beam!");
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnEnemyKilled();
        }

        if (_rb != null) _rb.linearVelocity = Vector2.zero;

        if (deadSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = deadSprite;
            _spriteRenderer.color = Color.gray;
        }

        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Core") && !_isDead)
        {
            CoreHealth coreHealth = other.GetComponentInParent<CoreHealth>();
            if (coreHealth != null)
            {
                coreHealth.TakeDamage(attackDamage);
            }
            Destroy(gameObject);
        }
    }
}