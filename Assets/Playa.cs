using UnityEngine;

public class Playa : MonoBehaviour
{
    public float speed = 5.0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 bewegung = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            bewegung.y += 1;
        if (Input.GetKey(KeyCode.S))
            bewegung.y -= 1;
        if (Input.GetKey(KeyCode.A))
            bewegung.x -= 1;
        if (Input.GetKey(KeyCode.D))
            bewegung.x += 1;

        // Bewegung
        transform.Translate(bewegung.normalized * speed * Time.deltaTime);

        // Animation ansteuern
        if (animator != null)
        {
            animator.SetFloat("Speed", bewegung.magnitude);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyAII enemy = collision.GetComponent<EnemyAII>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }

}
