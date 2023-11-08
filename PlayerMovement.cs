using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    #region Inputs

    private float horizontalInput; // horizontal input
    private float jumpInput; // jump input

    #endregion

    #region Move

    private bool canMove;
    float moveSpeed; // move speed

    float sprint;

    #endregion

    #region Jump

    float jumpForce; // force of jump

    float jumpTime; // jump time
    private float jumpTimeCounter; // jump time remaining

    float coioteTime; // time to jump after leaving the edge
    private float coioteTimeCounter; // time to jump after leaving the edge remaining

    private bool canGround;
    private bool isGrounded; // if character is touching the ground

    #endregion

    #region Powers
    private static string[] powerUps = { "WallJump", "IDK" };

    private static bool canWallJump;
    #endregion

    #region Others

    public Animator animations;
    private BoxCollider2D bc; // game object collision
    private Rigidbody2D rb; // game object rigid body ( psysics )

    #endregion

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponent<Animator>();

        canMove = true;
        moveSpeed = 2 * 3.5f;
        sprint = 1.5f;

        jumpForce = 10f; // 3.1 blocks
        jumpTime = .17f;
        jumpTimeCounter = jumpTime;

        coioteTime = .1f;
        coioteTimeCounter = coioteTime;

        isGrounded = false;
    }

    private void FixedUpdate()
    {
        AutoUpdateVariables();
        Movement();
        MyDebug();
    }

    private void MyDebug()
    {
        Debug.Log("Velocidade: " + rb.velocity.x / 2);
    }

    private void AutoUpdateVariables()
    {
        GetInputs();
        GetInteractions();
        NextLevel();
        Animation();
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetAxisRaw("Jump");
    }

    private void GetInteractions()
    {
        if (canGround)
        {
            isGrounded = Physics2D.OverlapBox(new Vector2(bc.bounds.center.x, bc.bounds.min.y), new Vector2(bc.size.x - .05f, .1f), .1f, LayerMask.GetMask("Ground"));
        }
    }

    private void NextLevel()
    {
        if(GameObject.Find("Final"))
        {
            if (bc.IsTouching(GameObject.Find("Final").GetComponent<BoxCollider2D>()))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void Animation()
    {
        if(horizontalInput != 0)
        {
            animations.SetBool("IsRunning", true);
            animations.SetBool("IsIdle", false);
        }
        else
        {
            animations.SetBool("IsIdle", true);
            animations.SetBool("IsRunning", false);
        }
    }

    private void Movement()
    {
        if (canMove)
        {
            HorizontalMove();
        }

        Jump();
    }

    private void HorizontalMove()
    {
        if(Input.GetAxisRaw("Sprint") == 1)
        {
            moveSpeed *= sprint;
        }
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        Faced();
    }

    private void Faced()
    {
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Jump()
    {
        JumpDetector();
        JumpTimer();
        JumpReseter();
    }

    private void JumpDetector()
    {
        if (jumpTimeCounter > 0 && jumpInput > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(jumpForce));
            canGround = false;
        }
        else if (coioteTimeCounter > 0 && jumpInput > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(jumpForce) * 1.5f);
            canGround = false;
        }
    }

    private void JumpTimer()
    {
        if (jumpTimeCounter > 0)
        {
            jumpTimeCounter -= Time.deltaTime;
        }
        if (coioteTimeCounter > 0)
        {
            coioteTimeCounter -= Time.deltaTime;
        }
    }

    private void JumpReseter()
    {
        if (!isGrounded && jumpInput <= 0)
        {
            jumpTimeCounter = 0;
            canGround = true;
        }
        if (isGrounded)
        {
            jumpTimeCounter = jumpTime;
            coioteTimeCounter = coioteTime;
            canGround = true;
        }
    }
}
