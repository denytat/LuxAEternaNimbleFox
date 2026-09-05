using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("References")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public Transform coreTransform;
    public CoreHealth coreHealth;

    [Header("Spawn Settings")]
    public float spawnRadius = 12f;

    [Header("Wave Tracking")]
    public int currentWave = 1;
    public int enemiesKilledInCurrentWave = 0;
    public int enemiesRequiredPerWave = 10;

    private float _enemySpeedMultiplier = 1f;
    private float _spawnInterval = 2f;
    private float _spawnTimer = 0f;
    private bool _bossSpawnedThisWave = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ApplyWaveModifiers();
    }

    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        if (currentWave >= 10 && !_bossSpawnedThisWave && bossPrefab != null)
        {
            _bossSpawnedThisWave = true;
            SpawnAtRadius(bossPrefab);
        }

        SpawnAtRadius(enemyPrefab);
    }

    private void SpawnAtRadius(GameObject prefab)
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = coreTransform != null ? coreTransform.position + (Vector3)randomCircle : (Vector3)randomCircle;
        
        GameObject spawnedObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        
        Enemy enemyScript = spawnedObj.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.moveSpeed *= _enemySpeedMultiplier;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilledInCurrentWave++;

        if (enemiesKilledInCurrentWave >= enemiesRequiredPerWave)
        {
            AdvanceWave();
        }
    }

    private void AdvanceWave()
    {
        currentWave++;
        enemiesKilledInCurrentWave = 0;
        _bossSpawnedThisWave = false;

        if (coreHealth != null)
        {
            coreHealth.Heal(10f);
        }

        ApplyWaveModifiers();
        Debug.Log($"--- ADVANCING TO WAVE {currentWave} ---");
    }

    private void ApplyWaveModifiers()
    {
        if (currentWave <= 5)
        {
            _enemySpeedMultiplier = 1f + ((currentWave - 1) * 0.15f);
        }

        if (currentWave > 4)
        {
            _spawnInterval = Mathf.Max(0.5f, 2f - ((currentWave - 4) * 0.25f));
        }
        else
        {
            _spawnInterval = 2f;
        }
    }

    public float GetCurrentAllyHealAmount()
    {
        return (currentWave > 4) ? 2.5f : 5.0f;
    }
}