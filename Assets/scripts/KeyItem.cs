using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [field: SerializeField] public string KeyId { get; private set; }

    private void Reset()
    {
        TakeableObject obj = GetComponent<TakeableObject>();
        if (obj == null)
        {
            obj = gameObject.AddComponent<TakeableObject>();
        }

        Debug.Log("KeyItem added. Set ObjectType to Key and ObjectId to match this key in Inspector.");
    }
}
