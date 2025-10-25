using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;

    void Update()
    {
        // Bewegung
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = new Vector2(moveX, moveY).normalized;

        animator.SetBool("IsMoving", moveDir.magnitude > 0);

        // Angriff per Mausklick
        if (Input.GetMouseButtonDown(0)) // Linke Maustaste
        {
            animator.SetTrigger("Attack");
        }
    }
}
