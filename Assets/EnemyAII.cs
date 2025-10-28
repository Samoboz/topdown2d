using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAII : MonoBehaviour
{
    [Header("Wanderverhalten")]
    public float wanderRadius = 3f;
    public float wanderInterval = 2f;
    public float wanderSpeed = 1.5f;

    [Header("Verfolgungsverhalten")]
    public float detectionRadius = 5f;
    public float chaseSpeed = 3f;
    public Transform player;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 wanderTarget;
    private float wanderTimer;
    private enum State { Wandering, Chasing, Dead }
    private State state = State.Wandering;

    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        ChooseNewWanderTarget();
    }

    private void Update()
    {
        if (isDead || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
            state = State.Chasing;
        else
            state = State.Wandering;

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        switch (state)
        {
            case State.Wandering:
                DoWander();
                break;
            case State.Chasing:
                DoChase();
                break;
        }
    }

    private void DoWander()
    {
        wanderTimer -= Time.fixedDeltaTime;

        if (wanderTimer <= 0f || Vector2.Distance(transform.position, wanderTarget) < 0.2f)
        {
            ChooseNewWanderTarget();
            wanderTimer = wanderInterval;
        }

        Vector2 dir = (wanderTarget - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + dir * wanderSpeed * Time.fixedDeltaTime);
    }

    private void ChooseNewWanderTarget()
    {
        Vector2 offset = Random.insideUnitCircle * wanderRadius;
        wanderTarget = (Vector2)transform.position + offset;
    }

    private void DoChase()
    {
        if (player == null) return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * chaseSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimationState()
    {
        if (animator == null) return;

        bool isMoving = (state == State.Chasing || state == State.Wandering);
        animator.SetBool("isMoving", isMoving);
    }

    // Wird von der Hitbox oder einem anderen Skript aufgerufen
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        state = State.Dead;

        // Bewegung sofort stoppen
        rb.linearVelocity = Vector2.zero;

        // Collider deaktivieren, damit kein weiterer Treffer registriert wird
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Animation triggern (sofern vorhanden)
        if (animator != null)
            animator.SetTrigger("Die");

        // Gegner nach kurzer Zeit zerst�ren (optional)
        Destroy(gameObject, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
