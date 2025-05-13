using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float stopDistance = 1.5f;
    [SerializeField] protected int damage;
    public float attackRadius = 1.3f;
    public int maxHealth;
    public Animator animator;
    public Transform player;
    public Rigidbody2D rb;
    protected bool facingRight = true;
    public LayerMask attackLayer;
    public Transform attackPoint;

    protected virtual void Update()
    {
        if (maxHealth <= 0) { Die(); }
        HandleMovement();
    }

    protected void HandleMovement()
    {

        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x > 0 && !facingRight) Flip(true);
        else if (direction.x < 0 && facingRight) Flip(false);

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat("Walk", -1);
        }
        else
        {
            rb.velocity = new Vector2(direction.x * moveSpeed, 0f);
            animator.SetFloat("Walk", 1);
        }
    }

    protected void Flip(bool faceRight)
    {
        transform.eulerAngles = faceRight ? new Vector3(0, -180, 0) : new Vector3(0, 0, 0);
        facingRight = faceRight;
    }

    public virtual void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (hit)
        {
            Player playerScript = hit.GetComponent<Player>();
            if (playerScript != null)
                playerScript.TakeDamage(damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    public void TakeDamage(int damage = 1)
    {
        if (maxHealth <= 0) { return; }
        maxHealth -= 1;
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
        StartCoroutine(Disappear());
    }
    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1.3f);
        gameObject.SetActive(false);
    }



}