using UnityEngine;

public class Move : MonoBehaviour
{

    public Animator animator;
    public float jumpHeight = 10;
    private bool onGround = true;
    public Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private float movement;
    private bool faceright = true;
    void Update()
    {

        HandleMovement();

        movement = Input.GetAxis("Horizontal");
        if (faceright && movement < 0)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            faceright = false;
        }
        else if (!faceright && movement > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            faceright = true;
        }
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
            onGround = false;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            animator.SetBool("Jump", false);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, 0);

        // Move
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

}
