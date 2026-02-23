using UnityEngine;

public class CA5GameManager : MonoBehaviour
{
    [System.Serializable]
    public class AssemblySocketRequirement
    {
        public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;
        public string expectedObjectId;
    }

    [Header("Tutorial Gate")]
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private Behaviour[] gatedBehaviours;
    [SerializeField] private Collider[] gatedColliders;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable[] gatedInteractables;

    [Header("Assembly")]
    [SerializeField] private AssemblySocketRequirement[] assemblySockets;
    [SerializeField] private GameObject finalObject;
    [SerializeField] private Collider finalObjectCollider;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable finalObjectGrab;
    [SerializeField] private string finalObjectId = "FINAL_OBJECT";

    [Header("Completion UI")]
    [SerializeField] private Transform completionUiSpawnPoint;
    [SerializeField] private GameObject completionSpatialUiPrefab;

    private bool puzzleCompleted;
    private bool completionTriggered;

    private void Awake()
    {
        if (tutorialManager == null)
        {
            tutorialManager = FindFirstObjectByType<TutorialManager>();
        }
    }

    private void OnEnable()
    {
        if (tutorialManager != null)
        {
            tutorialManager.MovementTutorialCompleted.AddListener(OnMovementTutorialCompleted);
            ApplyGateState(tutorialManager.IsMovementTutorialComplete);
        }
        else
        {
            ApplyGateState(true);
            Debug.LogWarning("CA5GameManager: TutorialManager not found. CA5 interactions are unlocked by default.");
        }

        RegisterAssemblySocketEvents(true);
    }

    private void OnDisable()
    {
        if (tutorialManager != null)
        {
            tutorialManager.MovementTutorialCompleted.RemoveListener(OnMovementTutorialCompleted);
        }

        RegisterAssemblySocketEvents(false);
    }

    private void Start()
    {
        SetFinalObjectState(false);
        ValidateAssembly();
    }

    private void OnMovementTutorialCompleted()
    {
        ApplyGateState(true);
    }

    private void ApplyGateState(bool unlocked)
    {
        for (int i = 0; i < gatedBehaviours.Length; i++)
        {
            if (gatedBehaviours[i] != null)
            {
                gatedBehaviours[i].enabled = unlocked;
            }
        }

        for (int i = 0; i < gatedColliders.Length; i++)
        {
            if (gatedColliders[i] != null)
            {
                gatedColliders[i].enabled = unlocked;
            }
        }

        for (int i = 0; i < gatedInteractables.Length; i++)
        {
            if (gatedInteractables[i] != null)
            {
                gatedInteractables[i].enabled = unlocked;
            }
        }
    }

    private void RegisterAssemblySocketEvents(bool register)
    {
        for (int i = 0; i < assemblySockets.Length; i++)
        {
            if (assemblySockets[i].socket == null)
            {
                continue;
            }

            if (register)
            {
                assemblySockets[i].socket.selectEntered.AddListener(OnAssemblySocketChanged);
                assemblySockets[i].socket.selectExited.AddListener(OnAssemblySocketChangedExit);
            }
            else
            {
                assemblySockets[i].socket.selectEntered.RemoveListener(OnAssemblySocketChanged);
                assemblySockets[i].socket.selectExited.RemoveListener(OnAssemblySocketChangedExit);
            }
        }
    }

    private void OnAssemblySocketChanged(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        ValidateAssembly();
    }

    private void OnAssemblySocketChangedExit(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs args)
    {
        if (!puzzleCompleted)
        {
            ValidateAssembly();
        }
    }

    private void ValidateAssembly()
    {
        if (puzzleCompleted || assemblySockets == null || assemblySockets.Length == 0)
        {
            return;
        }

        for (int i = 0; i < assemblySockets.Length; i++)
        {
            AssemblySocketRequirement requirement = assemblySockets[i];

            if (requirement.socket == null || !requirement.socket.hasSelection)
            {
                return;
            }

            UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selected = requirement.socket.firstInteractableSelected;
            if (selected == null)
            {
                return;
            }

            TakeableObject obj = selected.transform.GetComponent<TakeableObject>();
            if (obj == null || obj.ObjectId != requirement.expectedObjectId || obj.ObjectType != TakeableObjectType.PuzzlePiece)
            {
                return;
            }
        }

        puzzleCompleted = true;
        SetFinalObjectState(true);
    }

    private void SetFinalObjectState(bool enabled)
    {
        if (finalObject != null)
        {
            finalObject.SetActive(enabled);
        }

        if (finalObjectCollider != null)
        {
            finalObjectCollider.enabled = enabled;
        }

        if (finalObjectGrab != null)
        {
            finalObjectGrab.enabled = enabled;
        }
    }

    public bool IsFinalObject(TakeableObject obj)
    {
        return obj != null && obj.ObjectType == TakeableObjectType.FinalArtifact && obj.ObjectId == finalObjectId;
    }

    public void TriggerCompletion()
    {
        if (completionTriggered)
        {
            return;
        }

        completionTriggered = true;

        if (completionSpatialUiPrefab != null && completionUiSpawnPoint != null)
        {
            Instantiate(completionSpatialUiPrefab, completionUiSpawnPoint.position, completionUiSpawnPoint.rotation);
        }
    }
}
