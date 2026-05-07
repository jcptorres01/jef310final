using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject attackHitbox;

    public float attackCooldown = 2f;
    public float attackDuration = 0.5f;

    private bool canAttack = true;

    public void TryAttack()
    {
        if (canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;

        // TURN HITBOX ON
        attackHitbox.SetActive(true);

        Debug.Log("Enemy Attacking");

        yield return new WaitForSeconds(attackDuration);

        // TURN HITBOX OFF
        attackHitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}