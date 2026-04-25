using UnityEngine;
using System.Collections;

public class StatueRotatedDoorOpen : MonoBehaviour, IInteract
{
    [Header("References")]
    public StatueRotator statue;
    public LoadNextScene sceneLoader;

    [Header("Required Rotation")]
    public float requiredYRotation = 90f;
    public float tolerance = 5f;

    [Header("Door Movement")]
    public float moveDistance = 3f;   // how far down the door moves
    public float moveSpeed = 2f;      // speed of movement

    private bool unlocked = false;

    public void Interacting()
    {
        if (unlocked) return;

        if (IsStatueCorrect())
        {
            UnlockDoor();
        }
        else
        {
            Debug.Log("Statue is not in the correct position.");
        }
    }

    bool IsStatueCorrect()
    {
        float y = statue.GetYRotation();

        return Mathf.Abs(Mathf.DeltaAngle(y, requiredYRotation)) <= tolerance;
    }

    void UnlockDoor()
    {
        unlocked = true;

        Debug.Log("Door unlocked via statue puzzle!");

        if (sceneLoader != null)
        {
            sceneLoader.Interacting();
        }
        else
        {
            StartCoroutine(LowerDoor());
        }
    }

    IEnumerator LowerDoor()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.down * moveDistance;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }
}