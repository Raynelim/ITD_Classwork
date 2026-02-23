using UnityEngine;

public enum TakeableObjectType
{
    Key,
    PuzzlePiece,
    FinalArtifact
}

public class TakeableObject : MonoBehaviour
{
    [field: SerializeField] public string ObjectId { get; private set; }
    [field: SerializeField] public TakeableObjectType ObjectType { get; private set; }
}
