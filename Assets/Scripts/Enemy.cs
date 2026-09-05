using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float moveSpeed = 2f;
    public float maxHealth = 100f;
    public float attackDamage = 10f;

    [Header("Death & Conversion Visuals")]
    public Sprite deadSprite;
    public Sprite allySprite;

    [Header("Ally Conversion Settings")]
    public float timeInLightToConvert = 1.0f;
    public float allyMoveSpeed = 3.5f;
    public float coreHealAmount = 5f;

    private Transform _coreTransform;
    private float _currentHealth;
    private float _lightTimer = 0f;
    private bool _isInLight = false;
    private bool _isDead = false;
    private bool _isConvertedToAlly = false;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Rigidbody2D _rb;

    void Start()
    {
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();

        GameObject coreObj = GameObject.FindGameObjectWithTag("Core");
        if (coreObj != null)
        {
            _coreTransform = coreObj.transform;
        }
    }

    void Update()
    {
        if (_isConvertedToAlly)
        {
            MoveTowardCore(allyMoveSpeed);
            return;
        }

        if (_isDead)
        {
            if (_isInLight && LightbeamController.IsBeamActive)
            {
                _lightTimer += Time.deltaTime;

                if (_spriteRenderer != null)
                {
                    float progress = _lightTimer / timeInLightToConvert;
                    _spriteRenderer.color = Color.Lerp(Color.gray, Color.cyan, progress);
                }

                if (_lightTimer >= timeInLightToConvert)
                {
                    ConvertToAlly();
                }
            }
            else
            {
                _lightTimer = Mathf.Max(0f, _lightTimer - Time.deltaTime);
            }
            return;
        }

        MoveTowardCore(moveSpeed);

        if (_isInLight && LightbeamController.IsBeamActive)
        {
            TakeDamage(LightTriggerDetector._damage * Time.deltaTime);
        }
    }

    private void MoveTowardCore(float speed)
    {
        if (_coreTransform == null) return;

        Vector2 direction = (_coreTransform.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        if (_isDead || _isConvertedToAlly) return;

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

        if (_rb != null)
        {
            _rb.linearVelocity = Vector2.zero;
        }

        if (deadSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = deadSprite;
        }

        Destroy(gameObject, 3.0f);
    }

    private void ConvertToAlly()
    {
        _isConvertedToAlly = true;

        CancelInvoke();

        if (_collider != null)
        {
            _collider.enabled = true;
            _collider.isTrigger = true;
        }

        if (_spriteRenderer != null)
        {
            if (allySprite != null)
            {
                _spriteRenderer.sprite = allySprite;
            }
            _spriteRenderer.color = Color.cyan;
        }

        Debug.Log("Corpse charged and converted into an Ally!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightBeam"))
        {
            _isInLight = true;
        }

        if (other.CompareTag("Core"))
        {
            if (_isConvertedToAlly)
            {
                Debug.Log($"Core repaired by +{coreHealAmount} HP!");
                Destroy(gameObject);
            }
            else if (!_isDead)
            {
                Debug.Log("Core hit by enemy!");
                Destroy(gameObject);
            }
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