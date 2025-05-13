using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float attackRadius = 1f;
    public Animator animator;
    [SerializeField] private GameInput gameInput;
    public Image imageHealthBar;
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    private bool isDead = false;
    public Transform attackPoint;
    public LayerMask attackLayer;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        HandleAnimation();
        HandleAttack();
    }
    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Attack");
        }
    }
    private void HandleAnimation()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        animator.SetFloat("RunIdle", Mathf.Abs(inputVector.x) > 0.1f ? 1 : 0);
    }
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        imageHealthBar.fillAmount = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (hit)
        {
            EnemyBase enemyScript = hit.GetComponent<EnemyBase>();
            if (enemyScript != null)
                enemyScript.TakeDamage(1);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Dead");
        StartCoroutine(ReturnToMainMenu());
    }
    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("MainMenu");
        isDead = false;
    }
}
