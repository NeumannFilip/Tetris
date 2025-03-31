using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TetrominoController : MonoBehaviour
{
    [SerializeField] private float moveForce = 25f;
    [SerializeField] private float fastDropVelocity = 15f;
    public int PlayerId { get; set; }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
        rb.linearDamping = 5f;
    }

    public void Move(Direction direction)
    {
        Vector2 force = direction == Direction.Left ?
            Vector2.left * moveForce :
            Vector2.right * moveForce;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void FastDrop()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fastDropVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") ||
            collision.gameObject.CompareTag("Block"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            gameObject.tag = "Block";

            // Use the PlayerId set at spawn instead of position check
            GameManager.Instance.PieceLanded(gameObject, PlayerId);
        }
    }
}