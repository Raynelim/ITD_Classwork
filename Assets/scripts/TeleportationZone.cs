using UnityEngine;

public class TeleportAreaDetector : MonoBehaviour
{
    [SerializeField] private int areaNumber; // 1 or 2
    private TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindFirstObjectByType<TutorialManager>();
        if (tutorialManager == null)
        {
            Debug.LogError("TutorialManager not found! Make sure it exists in the scene.");
        }
        Debug.Log($"TeleportAreaDetector {areaNumber} initialized. Tutorial Manager found: {tutorialManager != null}");
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Teleport Area {areaNumber} - Collision detected with: {collision.gameObject.name} | Tag: {collision.tag}");
        
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Teleport Area {areaNumber} - PLAYER ENTERED!");
            if (tutorialManager != null)
            {
                tutorialManager.OnTeleportAreaEntered(areaNumber);
            }
            else
            {
                Debug.LogError($"Teleport Area {areaNumber} - TutorialManager is null!");
            }
        }
        else
        {
            Debug.Log($"Teleport Area {areaNumber} - Not player (tag is: {collision.tag})");
        }
    }
}