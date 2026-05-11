using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float secondsBeforeDestroy = 5f;
    float secondsElapsed = 0f;

    void Update()
    {
        secondsElapsed += Time.deltaTime;

        if (secondsElapsed >= secondsBeforeDestroy)
        {
            Destroy(this.gameObject);
        }
    }
}
