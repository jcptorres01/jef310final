using UnityEngine;

public class StatueRotator : MonoBehaviour, IInteract
{
    [Header("Rotation Settings")]
    public float rotationStep = 90f;
    public float rotationSpeed = 10f;

    private bool isRotating = false;

    public void Interacting()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateStatue());
        }
    }

    System.Collections.IEnumerator RotateStatue()
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(0, rotationStep, 0);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;
    }

    // Helper: get Y rotation in 0–360 range
    public float GetYRotation()
    {
        return transform.eulerAngles.y;
    }
}