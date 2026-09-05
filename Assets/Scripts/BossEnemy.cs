using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Boss Stats")]
    public float moveSpeed = 1.2f;
    public float maxHealth = 500f;
    public float attackDamage = 35f;

    [Header("Death Visuals")]
    public Sprite deadSprite;

    private Transform _coreTransform;
    private float _currentHealth;
    private bool _isInLight = false;
    private bool _isDead = false;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    void Start()
    {
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

        GameObject coreObj = GameObject.FindGameObjectWithTag("Core");
        if (coreObj != null)
        {
            _coreTransform = coreObj.transform;
        }
    }

    void Update()
    {
        if (_isDead) return;

        if (_coreTransform != null)
        {
            Vector2 direction = (_coreTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (_isInLight && LightbeamController.IsBeamActive)
        {
            TakeDamage(LightTriggerDetector._damage * Time.deltaTime);
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

        if (_rb != null)
        {
            _rb.linearVelocity = Vector2.zero;
        }

        if (deadSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = deadSprite;
        }

        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightBeam"))
        {
            _isInLight = true;
        }

        if (other.CompareTag("Core") && !_isDead)
        {
            CoreHealth coreHealth = other.GetComponentInParent<CoreHealth>();
            
            if (coreHealth != null)
            {
                coreHealth.TakeDamage(attackDamage);
            }

            Debug.Log($"Core hit by BOSS! Dealt {attackDamage} damage.");
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LightBeam"))
        {
            _isInLight = false;
        }
    }
}