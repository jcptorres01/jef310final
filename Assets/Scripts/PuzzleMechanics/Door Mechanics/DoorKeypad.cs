using UnityEngine;

public class DoorKeypad : MonoBehaviour, IInteract
{
    public GameObject keypadUI;
    public string correctCode = "1234";
    public int maxLength = 4;

    public LoadNextScene sceneLoader;
    private string currentInput = "";

    public PlayerMovementBehavior player;

    public KeypadUI keypadUIController;

    public void Interacting()
    {
        keypadUI.SetActive(true);

        player.SetMovementFrozen(true);
        PauseMenu.escLocked = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ResetInput();
    }

    public void PressNumber(string number)
    {
        if (currentInput.Length >= maxLength)
            return;

        currentInput += number;

        if (currentInput.Length == maxLength)
        {
            CheckCode();
        }
    }

    void CheckCode()
    {
        if (currentInput == correctCode)
        {
            CloseKeypad();
            UnlockDoor();
        }
        else
        {
            ResetInput();
        }
    }

    void UnlockDoor()
    {
        keypadUI.SetActive(false);

        // Restore gameplay
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        sceneLoader.Interacting();
        
    }

    public void CloseKeypad()
    {
        PauseMenu.escLocked = false;

        keypadUI.SetActive(false);

        Time.timeScale = 1f;

        player.SetMovementFrozen(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResetInput();

        // Tell UI to clear display
        if (keypadUIController != null)
        {
            keypadUIController.ClearDisplay();
        }
    }

    public void ResetInput()
    {
        currentInput = "";
    }
}