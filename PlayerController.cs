using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpInpulse = 8f;

    TouchingDirections touchingDirections;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
  


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();


    }




    public float CurrentMoveSpeed { get
        {
            if (canMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else { return walkSpeed; }
                    }
                    else
                    {
                        // air speed
                        return airWalkSpeed;
                    }

                }
                else
                {
                    // Idle speed is 0
                    return 0;
                }
            }else
            { 
                // movement locked
                return 0;
            }
          
        } }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);

        }
    }
    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);

        }
    }


    private bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set {
        if (_isFacingRight != value)
            {
                // flip the local Scale to make the player face the opposite direction
                transform.localScale *= new Vector2(-1, 1) ;
            }
             _isFacingRight = value; 
        } }

    public bool canMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } }
    public bool IsAlive { get { return animator.GetBool(AnimationStrings.isAlive); } }

   

    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
      
        if (IsAlive) 
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;

            SetFacingDeriction(moveInput);
        }else
        {
            IsMoving = false;
        }
       

    }

    private void SetFacingDeriction(Vector2 moveInput)
    {
      if(moveInput.x > 0 && !IsFacingRight)
        {
            // face the right
            IsFacingRight = true;
        }
      if(moveInput.x < 0 && IsFacingRight)
        {
            // face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && canMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpInpulse);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger(AnimationStrings.attackTrigger);
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
