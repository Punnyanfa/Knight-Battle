using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public DetectionZone biteDetectionZone;
    public float waypointReachedDistance = 0.1f;
    public float flightSpeed = 3f;
    public List<Transform> waypoints;
    Transform nextWayPoint;
    int waypointNum = 0;


    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;
    SpriteRenderer spriteRender;
    public Collider2D deathColider;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
        spriteRender = GetComponent<SpriteRenderer>();
        deathColider = GetComponent<Collider2D>();
    }

    public bool _hasTarget = false;


    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    private void Start()
    {
        nextWayPoint = waypoints[waypointNum];
    }
   /* private void OnEnable()
    {
        damageable.damageableDeath += OnDeath();
    }*/

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedCollider.Count > 0;
    }
    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {

        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void Flight()
    {
        // Fly to the next waypoint 
        Vector2 directionToWayPoint = (nextWayPoint.position - transform.position).normalized;

        // Check if we have reached the waypoint already 
        float distance = Vector2.Distance(nextWayPoint.position, transform.position);
        rb.velocity = directionToWayPoint * flightSpeed;
        UpdateDirection();

        // See if we need to switch waypoints 
        if (distance <= waypointReachedDistance)
        {
            // Switch to the next waypoint
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                // loop back to original waypoint 
                waypointNum = 0;

            }
            nextWayPoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            // Facing the right 
            if (rb.velocity.x < 0)
            {
                //Flip 
                //transform.localScale = new Vector3(-1 *localScale.x, localScale.y, localScale.z);
                spriteRender.flipX = true;
            }
            else
            {
                // Facing the left 
                if (rb.velocity.x > 0)
                {
                    //Flip 
                    // transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
                    spriteRender.flipX = false;
                }
            }

        }
    }
    public void OnDeath()
    {
        rb.gravityScale = 4f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathColider.enabled = true;
    }
}

