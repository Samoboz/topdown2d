using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    private Rigidbody2D rb;
    private EnemyAI ai;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        ai = GetComponent<EnemyAI>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} hat Schaden genommen. Aktuelle HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} ist besiegt.");

        // Bewegung stoppen
        if (ai != null)
            ai.enabled = false;

        if (rb != null)
            rb.velocity = Vector2.zero;

        // Optional: Animation oder Effekt
        // GetComponent<Animator>().SetTrigger("Death");

        // Optional: Gegner nach 2 Sekunden entfernen
        Destroy(gameObject, 2f);
    }
}
