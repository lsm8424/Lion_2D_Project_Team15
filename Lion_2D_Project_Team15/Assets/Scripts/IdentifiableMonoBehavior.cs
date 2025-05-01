using UnityEngine;

public abstract class IdentifiableMonoBehavior : MonoBehaviour
{
    [SerializeField] private string _objectId;
    public string ObjectID { get => _objectId; }

    // Save 기능 (미구현)
    public virtual void Save() { }
    public virtual void Load() { }
}
