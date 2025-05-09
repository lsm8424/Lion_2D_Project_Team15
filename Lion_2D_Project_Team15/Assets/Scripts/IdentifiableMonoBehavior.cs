using UnityEngine;

public abstract class IdentifiableMonoBehavior : MonoBehaviour
{
    [SerializeField] private string _objectId;
    public string ObjectID { get => _objectId; }
    public SaveManager.GameObjectEntry SaveData;
    // Save 기능 (미구현)
    public virtual void Load(SaveManager.GameObjectEntry saveInfo)
    {
        transform.position = saveInfo.Position;
        gameObject.SetActive(saveInfo.IsActive);
    }
}
