using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float moveSpeed = 1f;
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float beamDamagePerSecond = 200f;

    [Header("Death and Ally Visuals")]
    public Sprite deadSprite;
    public Sprite allySprite;

    [Header("Ally Settings")]
    public float timeInLightToConvert = 0.01f; 
    public float allyMoveSpeed = 3.5f;
    public float coreHealAmount = 5f;

    private Transform _coreTransform;
    private float _currentHealth;
    private float _lightTimer = 0f;
    private bool _isDead = false;
    private bool _isConvertedToAlly = false;
    private bool _touchingBeam = false;

    private Coroutine _despawnCoroutine;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _myCollider;

    void Start()
    {
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _myCollider = GetComponent<Collider2D>();

        GameObject coreObj = GameObject.FindGameObjectWithTag("Core");
        if (coreObj != null) _coreTransform = coreObj.transform;
    }

    void FixedUpdate()
    {
        // Physics overlap checks MUST happen in FixedUpdate for reliable collision detection
        _touchingBeam = CheckIfTouchingBeam();
    }

    void Update()
    {
        // 1. ALLY LOGIC: Walk to core and repair
        if (_isConvertedToAlly)
        {
            MoveTowardCore(allyMoveSpeed);
            return;
        }

        bool activeLightOnEnemy = _touchingBeam && LightbeamController.IsBeamActive;

        // 2. DEAD CORPSE LOGIC: Charge purification
        if (_isDead)
        {
            if (activeLightOnEnemy)
            {
                _lightTimer += Time.deltaTime;

                if (_spriteRenderer != null)
                {
                    float progress = Mathf.Clamp01(_lightTimer / timeInLightToConvert);
                    _spriteRenderer.color = Color.Lerp(Color.gray, Color.cyan, progress);
                }

                if (_lightTimer >= timeInLightToConvert)
                {
                    ConvertToAlly();
                }
            }
            else
            {
                // Slow decay instead of instant wipe so frame hitches don't ruin conversion
                _lightTimer = Mathf.Max(0f, _lightTimer - (Time.deltaTime * 0.5f));
            }
            return;
        }

        // 3. HOSTILE ENEMY LOGIC: Walk and take continuous damage
        MoveTowardCore(moveSpeed);

        if (activeLightOnEnemy)
        {
            TakeDamage(beamDamagePerSecond * Time.deltaTime);
        }
    }

    private bool CheckIfTouchingBeam()
    {
        if (_myCollider == null) return false;

        Collider2D[] hits = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        filter.useTriggers = true;

        int count = _myCollider.Overlap(filter, hits);
        for (int i = 0; i < count; i++)
        {
            if (hits[i] != null && hits[i].CompareTag("LightBeam"))
            {
                return true;
            }
        }
        return false;
    }

    private void MoveTowardCore(float speed)
    {
        if (_coreTransform == null) return;

        Vector2 direction = (_coreTransform.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnEnemyKilled();
        }

        if (deadSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = deadSprite;
            _spriteRenderer.color = Color.gray;
        }

        _despawnCoroutine = StartCoroutine(DespawnRoutine(4.0f)); // Generous 4s window
    }

    private IEnumerator DespawnRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!_isConvertedToAlly)
        {
            Destroy(gameObject);
        }
    }

    private void ConvertToAlly()
    {
        if (_isConvertedToAlly) return;
        _isConvertedToAlly = true;

        if (_despawnCoroutine != null)
        {
            StopCoroutine(_despawnCoroutine);
        }

        if (_spriteRenderer != null)
        {
            if (allySprite != null) _spriteRenderer.sprite = allySprite;
            _spriteRenderer.color = Color.cyan;
        }

        Debug.Log("Enemy successfully converted to Ally!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Core"))
        {
            CoreHealth coreHealth = other.GetComponentInParent<CoreHealth>();

            if (_isConvertedToAlly)
            {
                if (coreHealth != null) coreHealth.Heal(coreHealAmount);
                Destroy(gameObject);
            }
            else if (!_isDead)
            {
                if (coreHealth != null) coreHealth.TakeDamage(attackDamage);
                Destroy(gameObject);
            }
        }
    }

    public void ProcessLightExposure(float deltaDamage)
    {
        // 1. If converted to ally, ignore beam
        if (_isConvertedToAlly) return;

        // 2. If dead, charge purification
        if (_isDead)
        {
            _lightTimer += Time.deltaTime;

            if (_spriteRenderer != null)
            {
                float progress = Mathf.Clamp01(_lightTimer / timeInLightToConvert);
                _spriteRenderer.color = Color.Lerp(Color.gray, Color.cyan, progress);
            }

            if (_lightTimer >= timeInLightToConvert)
            {
                ConvertToAlly();
            }
            return;
        }

        // 3. If alive, take continuous damage
        TakeDamage(deltaDamage);
    }
}