using UnityEngine;

public class TriggerZoneDetector : MonoBehaviour
{
    [SerializeField] private int zoneNumber; // 1, 2, or 3
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private Transform playerTransform;
    private TutorialManager tutorialManager;
    private Transform xrOrigin;
    private bool hasTriggered = false;

    private void Start()
    {
        tutorialManager = FindFirstObjectByType<TutorialManager>();
        
        if (playerTransform != null)
        {
            xrOrigin = playerTransform;
        }
        else if (Camera.main != null)
        {
            xrOrigin = Camera.main.transform;
        }
        
        if (tutorialManager == null)
        {
            Debug.LogError("TutorialManager not found!");
        }
        if (xrOrigin == null)
        {
            Debug.LogError("XR Origin not found!");
        }
        
        Debug.Log($"TriggerZoneDetector {zoneNumber} initialized.");
    }

    private void Update()
    {
        if (xrOrigin == null || tutorialManager == null || hasTriggered)
            return;

        float distance = Vector3.Distance(transform.position, xrOrigin.position);
        
        if (distance < detectionRadius)
        {
            Debug.Log($"Zone {zoneNumber} - PLAYER DETECTED! Distance: {distance}");
            hasTriggered = true;
            tutorialManager.OnTriggerZoneEntered(zoneNumber);
        }
    }
}