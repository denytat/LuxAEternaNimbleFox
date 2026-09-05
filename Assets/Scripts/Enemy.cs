using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speed = 2f;
    public float _health = 100f;
    public float _attackDamage = 10f;

    public string _coretag = "Core";
    private Transform _coreTransform;
    private float _currentHealth;
    private bool _isInLight = false;
    // LightTriggerDetector _lightTriggerDetector = GetComponent<LightTriggerDetector>();
    
    void Start()
    {
        _currentHealth = _health;
        GameObject coreObject = GameObject.FindGameObjectWithTag(_coretag);
        if(coreObject != null)
        {
            _coreTransform = coreObject.transform;
        }
    }

    void Update()
    {
        if (_coreTransform != null)
        {
            Vector2 direction = (_coreTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * _speed * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (_isInLight)
            {
                // TakeDamage(_lightTriggerDetector._damage * Time.deltaTime);
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

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightBeam"))
        {
            _isInLight = true;
        }

        if(other.CompareTag("Core"))
        {
            // if(core != null)
            // {
            //     core.TakeDamage(_attackDamage);
            //     Die();
            // }
        }
    }
}
