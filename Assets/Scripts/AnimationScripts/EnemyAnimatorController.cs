using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public NavMeshAgent agent;
    public EnemyHealth health;

    private bool isDead = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (health == null)
            health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        UpdateMovementAnimation();
    }

    void UpdateMovementAnimation()
    {
        if (isDead) return;

        float speed = agent.velocity.magnitude;

        animator.SetFloat("SpeedAnim", speed);
    }

    // -----------------------------------
    // HIT ANIMATION
    // -----------------------------------

    public void PlayHit()
    {
        if (isDead) return;

        animator.SetBool("GetsHit", true);

        CancelInvoke(nameof(ResetHit));
        Invoke(nameof(ResetHit), 0.3f);
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