using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public PlayerMovementBehavior movement;
    public Rigidbody rb;
    public PlayerHealth health;

    private bool isDead = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (movement == null)
            movement = GetComponent<PlayerMovementBehavior>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateMovementAnimations();
    }

    void UpdateMovementAnimations()
    {
        // Horizontal movement only
        Vector3 flatVel =
            new Vector3(
                rb.velocity.x,
                0f,
                rb.velocity.z
            );

        float speed = flatVel.magnitude;

        animator.SetFloat("SpeedAnim", speed);

        // OPTIONAL:
        // If you later add sneak animation
        //animator.SetBool("InteractAnim", movement.isSneaking);
    }

    // -----------------------------------
    // PUBLIC ANIMATION TRIGGERS
    // -----------------------------------

    public void PlayAttackAnimation()
    {
        animator.SetBool("AttackAnim", true);

        CancelInvoke(nameof(ResetAttackAnim));
        Invoke(nameof(ResetAttackAnim), 0.5f);
    }

    void ResetAttackAnim()
    {
        animator.SetBool("AttackAnim", false);
    }

    public void PlayCameraAnimation()
    {
        animator.SetBool("UseCamAnim", true);

        CancelInvoke(nameof(ResetCameraAnim));
        Invoke(nameof(ResetCameraAnim), 1f);
    }

    void ResetCameraAnim()
    {
        animator.SetBool("UseCamAnim", false);
    }

    public void PlayPickupAnimation()
    {
        animator.SetBool("PickUpAnim", true);

        CancelInvoke(nameof(ResetPickupAnim));
        Invoke(nameof(ResetPickupAnim), 0.7f);
    }

    void ResetPickupAnim()
    {
        animator.SetBool("PickUpAnim", false);
    }

    public void PlayInteractAnimation()
    {
        animator.SetBool("InteractAnim", true);

        CancelInvoke(nameof(ResetInteractAnim));
        Invoke(nameof(ResetInteractAnim), 1f);
    }

    void ResetInteractAnim()
    {
        animator.SetBool("InteractAnim", false);
    }

    public void SetDead()
    {
        if (isDead)
            return;

        isDead = true;

        animator.SetBool("IsDead", true);
    }
}