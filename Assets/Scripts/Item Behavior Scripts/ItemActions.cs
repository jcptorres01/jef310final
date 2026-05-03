using UnityEngine;
using System.Collections;

public class ItemActions : MonoBehaviour
{
    public PlayerInventory player;

    [Header("Camera Dependencies")]
    public GameObject flashObject;

    [Header("Attacking Dependencies")]
    public GameObject AttackHitBox;

    // CAMERA ACTION
    public void TakePicture()
    {
        Debug.Log("Camera used!");

        StartCoroutine(Flash());

        // your camera logic here
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
        StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        AttackHitBox.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        AttackHitBox.SetActive(false);
    }
}