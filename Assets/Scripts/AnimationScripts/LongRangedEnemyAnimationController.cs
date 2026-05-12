using UnityEngine;
using UnityEngine.AI;

public class LongRangedEnemyAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public NavMeshAgent agent;
    public RangedEnemyHealth health;
    public EnemyShootingBehavior shooting;

    private bool isDead = false;
    private bool isAttacking = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (health == null)
            health = GetComponent<RangedEnemyHealth>();

        if (shooting == null)
            shooting = GetComponent<EnemyShootingBehavior>();
    }

    void Update()
    {
        if (isDead) return;

        UpdateMovement();
        UpdateShootingState();
    }

    // -----------------------------------
    // MOVEMENT ANIMATION
    // -----------------------------------
    void UpdateMovement()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("SpeedAnim", speed);
    }

    // -----------------------------------
    // SHOOTING / THROW STATE
    // -----------------------------------
    void UpdateShootingState()
    {
        if (shooting == null) return;

        bool isTryingToShoot = shooting.enabled;

        // Optional: only animate when actually in range/visible
        if (isTryingToShoot && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("AttackingThrow", true);
        }
        else if (!isTryingToShoot && isAttacking)
        {
            isAttacking = false;
            animator.SetBool("AttackingThrow", false);
        }
    }

    // -----------------------------------
    // HIT ANIMATION
    // -----------------------------------
    public void PlayHit()
    {
        if (isDead) return;

        animator.SetBool("GetsHit", true);

        CancelInvoke(nameof(ResetHit));
        Invoke(nameof(ResetHit), 0.25f);
    }

    void ResetHit()
    {
        animator.SetBool("GetsHit", false);
    }

    // -----------------------------------
    // DEATH ANIMATION
    // -----------------------------------
    public void PlayDeath()
    {
        if (isDead) return;

        isDead = true;

        animator.SetBool("Dies", true);
    }
}