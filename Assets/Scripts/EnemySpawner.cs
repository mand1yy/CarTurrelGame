using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform carTransform;

    [Header("Spawn Area Settings")]
    [SerializeField] private Vector3 spawnAreaCenter;
    [SerializeField] private Vector2 spawnAreaSize;

    [Header("Settings")]
    [SerializeField] private int numberOfEnemies = 10;

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float spawnX = spawnAreaCenter.x + Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float spawnZ = spawnAreaCenter.z + Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

            Vector3 spawnPosition = new Vector3(spawnX, spawnAreaCenter.y, spawnZ);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetTarget(carTransform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnAreaCenter, new Vector3(spawnAreaSize.x, 1, spawnAreaSize.y));
    }
}
