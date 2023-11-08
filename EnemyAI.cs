using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Move

    private float horizontalInput; // horizontal input
    float moveSpeed; // move speed
    float maxSpeed; // max move speed

    #endregion

    #region Jump

    float jumpForce; // force of jump

    float jumpTime; // jump time
    private float jumpTimeCounter; // jump time remaining

    bool isGrounded; // if character is touching the ground

    #endregion

    #region Others

    private BoxCollider2D bc; // game object collision
    private Rigidbody2D rb; // game object rigid body ( psysics )
    private GameObject player;

    #endregion

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        moveSpeed = 2f;
        maxSpeed = 2f;

        jumpForce = 10.2f;
        jumpTime = .2f;
        jumpTimeCounter = jumpTime;
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
    }

    private void AutoUpdateVariables()
    {
        isGrounded = Physics2D.OverlapBox(new Vector2(bc.bounds.center.x, bc.bounds.min.y), new Vector2(bc.size.x - .05f, .1f), .1f, LayerMask.GetMask("Ground"));

        horizontalInput = (player.transform.position.x - transform.position.x);

        if(horizontalInput > maxSpeed)
        {
            horizontalInput = maxSpeed;
        }
        if (horizontalInput < -maxSpeed)
        {
            horizontalInput = -maxSpeed;
        }
    }

    private void Movement()
    {
        HorizontalMove();
        Jump();
    }

    private void HorizontalMove()
    {
        if (transform.localScale.y < 1)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed * (2 - transform.localScale.y), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
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
        if (jumpTimeCounter > 0 && (player.transform.position.y > transform.position.y + .01f))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(jumpForce / 3 * 2));
        }
    }

    private void JumpTimer()
    {
        if (jumpTimeCounter > 0)
        {
            jumpTimeCounter -= Time.deltaTime;
        }
    }

    private void JumpReseter()
    {
        if (!isGrounded && (player.transform.position.y <= transform.position.y))
        {
            jumpTimeCounter = 0;
        }
        if (isGrounded)
        {
            jumpTimeCounter = jumpTime;
        }

    }
}
