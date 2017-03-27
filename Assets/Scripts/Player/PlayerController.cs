/*
Copyright (c) Mr BlckHrtzz
Let The Mind Dominate The Hrtzz
*/

using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables 
    Animator playerAnimator;
    Rigidbody2D rBody;
    public float jumpForce = 50f;           //How musch the player should jump.
    public Transform[] groundPoint;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;           //Set it to everything from editor.

    bool isJumping = false;
    bool isGrounded = false;

    public int totalJumpAllowed = 2;        //Total a jumps Allowed
    int jumpRemaining = 0;                  //Counter to check How Many Games are Made.


    #endregion

    #region Unity Functions

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

        if (Input.GetButtonDown("Jump") && isGrounded && jumpRemaining < totalJumpAllowed)
        {
            Jump();
            jumpRemaining++;
        }
        else if (!isGrounded && Input.GetButtonDown("Jump") && jumpRemaining < totalJumpAllowed)
        {
            Jump();
            jumpRemaining++;
        }
        else if (isGrounded)
        {
            jumpRemaining = 0;
        }

        if (!isGrounded)
        {
            playerAnimator.SetLayerWeight(1, 1);
        }
        else
            playerAnimator.SetLayerWeight(1, 0);

        if (rBody.velocity.y < 0)
        {
            playerAnimator.SetBool("Landing", true);
        }
    }

    #endregion

    #region UserDefined
    //Function To make Player jump.
    void Jump()
    {
        rBody.velocity = new Vector3(0, jumpForce, 0);
        playerAnimator.SetTrigger("Jump");
        isJumping = false;
    }
    #endregion

    //FUnction To Check if The Player is grounded or not.
    bool IsGrounded()
    {
        if (rBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoint)
            {
                Collider2D[] collider = Physics2D.OverlapCircleAll(point.position, groundRadius, groundLayer);
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].gameObject != gameObject)
                    {
                        playerAnimator.ResetTrigger("Jump");
                        playerAnimator.SetBool("Landing", false);
                        return true;
                    }
                }
            }

        }
        return false;
    }

}
