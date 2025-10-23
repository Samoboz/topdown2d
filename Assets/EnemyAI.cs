using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Wanderverhalten")]
    public float wanderRadius = 3f;
    public float wanderInterval = 2f;
    public float wanderSpeed = 1.5f;

    [Header("Verfolgungsverhalten")]
    public float detectionRadius = 5f;
    public float chaseSpeed = 3f;
    public Transform player;

    private Rigidbody2D rb;
    private Vector2 wanderTarget;
    private float wanderTimer;
    private enum State { Wandering, Chasing }
    private State state = State.Wandering;

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
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
            state = State.Chasing;
        else
            state = State.Wandering;
    }

    private void FixedUpdate()
    {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
