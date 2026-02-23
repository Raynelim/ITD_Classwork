using UnityEngine;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class PedestalSocketReceiver : MonoBehaviour
{
    [SerializeField] private CA5GameManager gameManager;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<CA5GameManager>();
        }
    }

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnPlaced);
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnPlaced);
    }

    private void OnPlaced(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (gameManager == null || args == null || args.interactableObject == null)
        {
            return;
        }

        TakeableObject obj = args.interactableObject.transform.GetComponent<TakeableObject>();
        if (gameManager.IsFinalObject(obj))
        {
            gameManager.TriggerCompletion();
        }
    }
}
