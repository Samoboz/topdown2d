using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [HideInInspector]
    public EnemySpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDestroyed();
        }
    }
}
