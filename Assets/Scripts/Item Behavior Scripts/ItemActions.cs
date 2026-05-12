using UnityEngine;
using System.Collections;

public class ItemActions : MonoBehaviour
{
    public PlayerInventory player;

    public PlayerAnimationController animController;

    [Header("Camera Dependencies")]
    public GameObject flashObject;

    [Header("Attacking Dependencies")]
    public GameObject AttackHitBox;

    // CAMERA ACTION
    public void TakePicture()
    {
        Debug.Log("Camera used!");

        if (animController != null)
        {
            animController.PlayCameraAnimation();
        }

        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        flashObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        flashObject.SetActive(false);
    }

    // Attacking
    public void Attacking()
    {
        if (animController != null)
        {
            animController.PlayAttackAnimation();
        }

        StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        AttackHitBox.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        AttackHitBox.SetActive(false);
    }
}