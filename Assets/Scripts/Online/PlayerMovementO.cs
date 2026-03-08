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
    [SerializeField] private float knockback;
    [SerializeField] private bool knockbacked;
    [SerializeField] private PlayerAttributesO attributes;
    [SerializeField] private Animator animator;


    public TMP_Text Results;
    public Button EndButton;

    private bool Jumping;
    //  private bool wait;
    public bool won;
    public bool lost;

    private float finishTime;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        jumpVector.y = jumpForce;
        canJump = true;
        movementVector.x = speed;
        checkpoint = transform.position;
       // wait = true;
        transform.position = GameManagerO.instance.playerSpawn.position;
        Results = GameManagerO.instance.EndTime;
        EndButton = GameManagerO.instance.EndButton;
        Results.text = "";
        EndButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    
         if (!IsOwner) return;
        if (GameManagerO.instance.Running)
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

            //Finish();
            GameManagerO.instance.OnFinishRpc();
            //    //if (won)
            //    //GameManagerO.instance.EndTime.text = "You Win!";
            //    //else
            //    //    GameManagerO.instance.EndTime.text = "You Lose!";
            //}
        }

        //public void Finish()
        //{
        //    EndButton.gameObject.SetActive(true);
        //    float id = 
        // //   GameManagerO.instance.Timer.text = "";
        //    if (GameManagerO.instance.CheckWon()
        //    {
        //        Results.text = "You Win!";
        //    }
        //    else
        //    {
        //        Results.text = "You Lose!";
        //    }
        //}
    }
}
