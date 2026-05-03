using UnityEngine;

public class BreakableObjectBehvaior : MonoBehaviour
{
    [Header("Break Settings")]
    public int hitsRequired = 2;

    private int currentHits = 0;

    [Header("Child Planks")]
    public GameObject plank1;
    public GameObject plank2;

    public void TakeHit(int damage)
    {
        currentHits += damage;

        Debug.Log(gameObject.name + " was hit!");

        // FIRST HIT
        if (currentHits == 1)
        {
            DropPlank(plank2);
        }

        // SECOND HIT
        if (currentHits == 2)
        {
            DropPlank(plank1);
        }

        // BREAK OBJECT
        if (currentHits >= hitsRequired)
        {
            BreakObject();
        }
    }

    void DropPlank(GameObject plank)
    {
        if (plank == null)
            return;

        // Remove parent so it can fall independently
        plank.transform.SetParent(null);

        Rigidbody rb = plank.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = plank.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
    }

    void BreakObject()
    {
        Debug.Log(gameObject.name + " broke!");

        gameObject.SetActive(false);

        // OR Destroy(gameObject);
    }
}