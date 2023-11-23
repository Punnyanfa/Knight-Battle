using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))] 

public class Knight : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirection;
    Animator animator;
    Damageable damageable;
    public enum WalkableDirection { Right, Left}
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
            if (_walkDirection != value) {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }if(value == WalkableDirection.Right)
            {
                walkDirectionVector = Vector2.right;
            }else if (value == WalkableDirection.Left)
            {
                walkDirectionVector = Vector2.left;
            }


            _walkDirection = value; }
    }
    private bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget; } private set {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        } }

    public bool CanMove { get {
            return animator.GetBool(AnimationStrings.canMove);
        } }

/*    public float AttackCoolDown { get
        {
            return animator.GetFloat(AnimationStrings.attackCoolDown);
        } private set
        {
            animator.SetFloat(AnimationStrings.attackCoolDown, Mathf.Max(value, 0));
        }
    }*/

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
       HasTarget = attackZone.detectedCollider.Count > 0;
    }
    private void FixedUpdate()
    {
        if(touchingDirection.IsGrounded && touchingDirection.IsOnWall)
        {
            FlipDirections();
        }
        if (!damageable.LockVelocity)
        {
         if(CanMove && touchingDirection.IsGrounded)
            {
                // Accelerate towards max Speed
                rb.velocity = new Vector2(
                Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                rb.velocity.y);
            }
               
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
          
        }
       
    }

    private void FlipDirections()
    {
     if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
       
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }




}
