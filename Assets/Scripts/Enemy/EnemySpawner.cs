using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnDelay = 10f;

    private GameObject currentEnemy;

    void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is not assigned in the EnemySpawner");
            return;
        }

        currentEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        currentEnemy.GetComponent<Animal>().OnDestroyed += HandleOnEnemyDeath;
    }

    void OnDestroy()
    {
        currentEnemy.GetComponent<Animal>().OnDestroyed -= HandleOnEnemyDeath;
    }

    private void HandleOnEnemyDeath()
    {
        currentEnemy = null;
        Invoke(nameof(SpawnEnemy), respawnDelay);
    }
}
