using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject triggerZone1;
    [SerializeField] private GameObject triggerZone2;
    [SerializeField] private GameObject triggerZone3;
    
    [SerializeField] private GameObject teleportArea1;
    [SerializeField] private GameObject teleportArea2;
    
    [SerializeField] private GameObject congratsMessage;
    [SerializeField] private Button dismissButton; // Add an X button in your UI
    
    private int triggersCompleted = 0;
    private bool teleport1Completed = false;
    private bool teleport2Completed = false;

    public bool IsMovementTutorialComplete { get; private set; }
    public UnityEvent MovementTutorialCompleted;

    private void Start()
    {
        // Enable only trigger zone 1 at start
        triggerZone1.SetActive(true);
        triggerZone2.SetActive(false);
        triggerZone3.SetActive(false);
        
        // Disable teleport areas initially
        teleportArea1.SetActive(false);
        teleportArea2.SetActive(false);
        
        // Hide congrats message
        congratsMessage.SetActive(false);
        
        // Setup dismiss button
        if (dismissButton != null)
        {
            dismissButton.onClick.AddListener(DismissMessage);
        }
        else
        {
            Debug.LogWarning("Dismiss button not assigned in TutorialManager!");
        }

        IsMovementTutorialComplete = false;
    }

    public void OnTriggerZoneEntered(int zoneNumber)
    {
        if (zoneNumber == 1 && triggersCompleted == 0)
        {
            triggersCompleted = 1;
            triggerZone1.SetActive(false);
            triggerZone2.SetActive(true);
            Debug.Log("Trigger Zone 1 completed. Zone 2 enabled.");
        }
        else if (zoneNumber == 2 && triggersCompleted == 1)
        {
            triggersCompleted = 2;
            triggerZone2.SetActive(false);
            triggerZone3.SetActive(true);
            Debug.Log("Trigger Zone 2 completed. Zone 3 enabled.");
        }
        else if (zoneNumber == 3 && triggersCompleted == 2)
        {
            triggersCompleted = 3;
            triggerZone3.SetActive(false);
            // Enable teleport areas sequentially
            teleportArea1.SetActive(true);
            Debug.Log("All trigger zones completed. Teleport Area 1 enabled.");
        }
    }

    public void OnTeleportAreaEntered(int areaNumber)
    {
        if (areaNumber == 1 && !teleport1Completed)
        {
            teleport1Completed = true;
            teleportArea1.SetActive(false);
            teleportArea2.SetActive(true);
            Debug.Log("Teleport Area 1 completed. Area 2 enabled.");
        }
        else if (areaNumber == 2 && teleport1Completed && !teleport2Completed)
        {
            teleport2Completed = true;
            teleportArea2.SetActive(false);
            IsMovementTutorialComplete = true;
            MovementTutorialCompleted?.Invoke();
            ShowCongratsMessage();
            Debug.Log("Tutorial completed!");
        }
    }

    private void ShowCongratsMessage()
    {
        congratsMessage.SetActive(true);
    }

    public void DismissMessage()
    {
        congratsMessage.SetActive(false);
        // Optional: Load next scene or continue game
    }
}