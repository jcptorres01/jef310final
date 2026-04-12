using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour, IInteract
{
    public string titleScreen = "TitleScreen"; // NEW - assign your main menu scene name

    public void Interacting()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else // NEW
        {
            SceneManager.LoadScene(titleScreen); // NEW
        }
    }
}