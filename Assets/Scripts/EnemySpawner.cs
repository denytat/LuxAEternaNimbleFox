using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject _enemyPrefab;
    public float _spawnInterval = 2f;
    private float _spawnDistance = 7f;

    private Coroutine _coroutine;

    void Start()
    {
        _coroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spawnPosition = new Vector3(
            Mathf.Cos(randomAngle) * _spawnDistance,
            Mathf.Sin(randomAngle) * _spawnDistance,
            0f
        );
        Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public void StopSpawning()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
}
