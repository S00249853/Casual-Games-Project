using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerMovementO : NetworkBehaviour
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
    [SerializeField] private PlayerAttackO attack;
    [SerializeField] private Animator animator;


    private bool Jumping;

    public NetworkVariable<GameManagerO.Player> player = new NetworkVariable<GameManagerO.Player>( GameManagerO.Player.Player1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        jumpVector.y = jumpForce;
        canJump = true;
        movementVector.x = speed;
        checkpoint = transform.position;
        transform.position = GameManagerO.instance.playerSpawn.position;


        
    }

    void Update()
    {
        if (GameManagerO.instance.Starting)
            player.Value = GameManagerO.instance.GetLocalPlayer();
        if (!IsOwner) return;
        if (GameManagerO.instance.Running)
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

        if (collision.gameObject.CompareTag("Hazard"))
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
            GameManagerO.instance.OnFinish(player.Value);
        }
    }
}
