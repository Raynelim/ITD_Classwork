using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class DoorLockSocket : MonoBehaviour
{
    [SerializeField] private string expectedKeyId;
    [SerializeField] private GameObject physicalLockVisual;
    [SerializeField] private GameObject unlockedVisual;
    [SerializeField] private GameObject hiddenStoredObject;
    [SerializeField] private bool keepKeyInSocket = true;

    [Header("What unlocks")]
    [SerializeField] private Behaviour[] behavioursToEnableOnUnlock;
    [SerializeField] private Collider[] collidersToEnableOnUnlock;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable[] interactablesToEnableOnUnlock;

    public UnityEvent OnUnlocked;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    private bool isUnlocked;

    private void Awake()
    {
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    private void Start()
    {
        SetLockedVisualState(true);

        if (hiddenStoredObject != null)
        {
            hiddenStoredObject.SetActive(false);
        }

        SetUnlockTargets(false);
    }

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnKeyInserted);
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnKeyInserted);
    }

    private void OnKeyInserted(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (isUnlocked || args == null || args.interactableObject == null)
        {
            return;
        }

        KeyItem keyItem = args.interactableObject.transform.GetComponent<KeyItem>();
        if (keyItem != null)
        {
            if (keyItem.KeyId != expectedKeyId)
            {
                return;
            }
        }
        else
        {
            TakeableObject key = args.interactableObject.transform.GetComponent<TakeableObject>();
            if (key == null || key.ObjectType != TakeableObjectType.Key || key.ObjectId != expectedKeyId)
            {
                return;
            }
        }

        Unlock();

        if (!keepKeyInSocket)
        {
            socketInteractor.EndManualInteraction();
        }
    }

    private void Unlock()
    {
        isUnlocked = true;
        SetLockedVisualState(false);
        SetUnlockTargets(true);

        if (hiddenStoredObject != null)
        {
            hiddenStoredObject.SetActive(true);
        }

        OnUnlocked?.Invoke();
    }

    private void SetLockedVisualState(bool locked)
    {
        if (physicalLockVisual != null)
        {
            physicalLockVisual.SetActive(locked);
        }

        if (unlockedVisual != null)
        {
            unlockedVisual.SetActive(!locked);
        }
    }

    private void SetUnlockTargets(bool enabled)
    {
        for (int i = 0; i < behavioursToEnableOnUnlock.Length; i++)
        {
            if (behavioursToEnableOnUnlock[i] != null)
            {
                behavioursToEnableOnUnlock[i].enabled = enabled;
            }
        }

        for (int i = 0; i < collidersToEnableOnUnlock.Length; i++)
        {
            if (collidersToEnableOnUnlock[i] != null)
            {
                collidersToEnableOnUnlock[i].enabled = enabled;
            }
        }

        for (int i = 0; i < interactablesToEnableOnUnlock.Length; i++)
        {
            if (interactablesToEnableOnUnlock[i] != null)
            {
                interactablesToEnableOnUnlock[i].enabled = enabled;
            }
        }
    }
}
