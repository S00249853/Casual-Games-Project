using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Vector2 jumpVector;
    private Vector2 movementVector;
    private Vector2 checkpoint;
    public float speed;

    [SerializeField] private float jumpForce;
    [SerializeField] private bool canJump;
    [SerializeField] private float jumpSlope;
    [SerializeField] private bool canWallJump;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private Animator animator;
    [SerializeField] private GameManager manager;
    

    private bool Jumping;
    public bool Wait;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        jumpVector.y = jumpForce;
        canJump = true;
        movementVector.x = speed;
        checkpoint = transform.position;
        Wait = true;
    }

    void Update()
    {
        if (!Wait)
        {
            movementVector.y = body.linearVelocityY;

            body.linearVelocity = movementVector;
    
        }
        animator.SetFloat("xVelocity", Mathf.Abs(body.linearVelocityX));
        animator.SetBool("Jumping", Jumping);
    }

    public void OnJump()
    {
        if (canJump == true)
        {
            Jumping = true;
            body.AddForce(jumpVector, ForceMode2D.Impulse);
        }

        if (canWallJump == true)
        {
            Jumping = true;
            movementVector.x = -movementVector.x;
            body.AddForce(jumpVector, ForceMode2D.Impulse);
            if (transform.localScale.x == 4)
            {
                transform.localScale = new Vector3(-4, 4, 1);
            }
            else
            {
                transform.localScale = new Vector3(4, 4, 1);
            }
        }


    }


    protected void WallJump()
    {
        Jumping = true;
        movementVector.x = -movementVector.x; 
        body.AddForce(jumpVector, ForceMode2D.Impulse);
        if (transform.localScale.x == 4)
        {
            transform.localScale = new Vector3(-4, 4, 1);
        }
        else
        {
            transform.localScale = new Vector3(4, 4, 1);
        }

    }

    public void OnJumpingCompleted()
    {
        Jumping = false;
    }

    private void CheckCanJump(Vector2 surfaceNormal)
    {
        canJump = Vector2.Dot(surfaceNormal, Vector2.up) >= jumpSlope;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contactCount > 0)
        {
            CheckCanJump(collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            canWallJump = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.position = checkpoint;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contactCount > 0)
        {
            CheckCanJump(collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            canWallJump = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        canWallJump = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            checkpoint = collision.gameObject.transform.position;
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            manager.OnFinish();
        }
    }
}
