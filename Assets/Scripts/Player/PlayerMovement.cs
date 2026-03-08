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
    [SerializeField] private float knockback;
    [SerializeField] private bool knockbacked;
    [SerializeField] private PlayerAttributes attributes;
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

    // Update is called once per frame
    void Update()
    {
        // if (!IsOwner) return;
        if (!Wait)
        {
            movementVector.y = body.linearVelocityY;

            body.linearVelocity = movementVector;

            if (Input.GetButtonDown("Jump"))
            {
                if (canJump == true)
                {
                    Jump();
                    // canJump = false;
                }

                if (canWallJump == true)
                {
                    WallJump();
                    canWallJump = false;
                }
            }

           
        }
        animator.SetFloat("xVelocity", Mathf.Abs(body.linearVelocityX));
        animator.SetBool("Jumping", Jumping);
    }

    void Knockback()
    {
        knockbacked = true;
        movementVector.x = -movementVector.x;
        body.AddForce(new Vector2(jumpVector.x,knockback), ForceMode2D.Impulse);


        movementVector.x = -movementVector.x;
        knockbacked = true;

    }

    protected void Jump()
    {
        Jumping = true;
        body.AddForce(jumpVector, ForceMode2D.Impulse);
    
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
            attributes.Health -= 10;
            Knockback();
        }

        if (collision.gameObject.CompareTag("Hazard"))
        {
            attributes.Health -= 10;
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
        //canJump = false;
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
          //  EndTime.text = timer.ToString();
        }
    }
}
