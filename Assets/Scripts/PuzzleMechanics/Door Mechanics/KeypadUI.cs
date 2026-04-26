using UnityEngine;
using TMPro;

public class KeypadUI : MonoBehaviour
{
    [Header("References")]
    public DoorKeypad doorKeypad; // reference to your door script
    public TextMeshProUGUI displayText;

    public KeyCode removeUI = KeyCode.Escape;

    private string displayInput = "";

    void Update()
    {
        // Only listen when keypad is active
        if (!gameObject.activeInHierarchy) return;

        if (Input.GetKeyDown(removeUI))
        {
            CloseKeypad();
        }
    }

    // Called by buttons (1–9)
    public void PressNumber(string number)
    {
        doorKeypad.PressNumber(number);

        // Update local display (optional but recommended)
        if (displayInput.Length < doorKeypad.maxLength)
        {
            displayInput += number;
            UpdateDisplay();
        }

        // Reset display if max length reached (DoorKeypad already checked it)
        if (displayInput.Length == doorKeypad.maxLength)
        {
            Invoke(nameof(ClearDisplay), 0.2f); // small delay for feedback
        }
    }

    public void ClearDisplay()
    {
        displayInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        displayText.text = displayInput;
    }

    // Optional: close button on UI
    public void CloseKeypad()
    {
        doorKeypad.CloseKeypad();
    }


}