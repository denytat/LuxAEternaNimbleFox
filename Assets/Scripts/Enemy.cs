using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speed = 2f;
    public float _health = 100f;
    public float _attackDamage = 5f;

    public string _coretag = "Core";
    private Transform _coreTransform;
    private float _currentHealth;
    private bool _isInLight = false;

    public Sprite _deadSprite;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private bool _isDead = false;

    private Rigidbody2D _rb;

    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        if (_isDead) return;
        _currentHealth = _health;
        GameObject coreObject = GameObject.FindGameObjectWithTag(_coretag);
        if(coreObject != null)
        {
            _coreTransform = coreObject.transform;
        }
    }

    void Update()
    {
        if(_isDead) return;
        if (_coreTransform != null)
        {
            Vector2 direction = (_coreTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * _speed * Time.deltaTime;

            if (_isInLight)
            {
                TakeDamage(LightTriggerDetector._damage * Time.deltaTime);
            }
        }
    }

    void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        _isDead = true;
        _spriteRenderer.sprite = _deadSprite;
        _collider.enabled = false;

        if (_rb != null)
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightBeam"))
        {
            _isInLight = true;
        }

        if (other.CompareTag("Core"))
        {
            Attack();
            Destroy(gameObject);
        }
    }
    
    void Attack()
    {
        Core._coreHealth -= _attackDamage;
    }
}
