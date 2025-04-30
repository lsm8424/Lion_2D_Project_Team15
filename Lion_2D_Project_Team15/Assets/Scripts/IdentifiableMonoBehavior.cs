using UnityEngine;

public abstract class IdentifiableMonoBehavior : MonoBehaviour
{
    [SerializeField] private string _objectId;
    public string ObjectId { get => _objectId; }
}
