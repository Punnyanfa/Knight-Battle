using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChange;

    Animator animator;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth { get 
        {
           return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
        }
    [SerializeField]
    private int _health = 100;
    public int Health { get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChange?.Invoke(_health, MaxHealth);

            // If health drops below 0 character is no longer alive
            if (_health <= 0)
            {
                IsAlive = false;

            }
        }

    }
   
    [SerializeField]
    private bool isInvincible = false;

   

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;
    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive { get { return _isAlive; } private set {
        _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("Is Alive set " + value);

            if (value == false)
            {
                damageableDeath.Invoke();
            }
        } }
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isInvincible) 
        {
            if (timeSinceHit > invincibilityTime)
            {
                // Remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;

        }
     
    }
   
    // Returns whether the damageable took damage or not 
    public bool Hit (int damage, Vector2 knockback)
    {
        if (_isAlive && !isInvincible) 
        {
            Health -= damage;
            isInvincible = true;

            // Notify other subcribed component that the damageable was hit to handle the knockback and such
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage,  knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;

        }
        // Unable to hit
        return false;
    }
    public bool Heal(int healthReStore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthReStore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
