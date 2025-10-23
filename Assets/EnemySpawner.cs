using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Einstellungen")]
    public GameObject enemyPrefab;     // Prefab des Gegners
    public int maxEnemies = 10;        // Maximale Anzahl gleichzeitig gespawnter Gegner

    [Header("Spawn Bereich")]
    public float spawnRadius = 5f;     // Radius um den Spawner, in dem Gegner erscheinen

    [Header("Spawn Timing")]
    public float spawnInterval = 3f;   // Zeit zwischen Spawns (in Sekunden)

    private float timer = 0f;
    private int currentEnemies = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        // Zufällige Position im Kreis um den Spawner
        Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        // Gegner erzeugen
        GameObject newEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        // Mitzählen (optional)
        currentEnemies++;

        // Gegner zerstört? → mitzählen verringern
        EnemyDeathHandler deathHandler = newEnemy.AddComponent<EnemyDeathHandler>();
        deathHandler.spawner = this;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemies--;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
