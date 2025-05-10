using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "GameObjectActiveEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/GameObjectActiveEventFunction_SO"
)]
public class GameObjectActiveEventFunction_SO : EventFunction_SO
{
    public string ObjectID;
    public bool SetActiveTo;

    private GameObject target;

    public override void Setup()
    {
        if (!IDManager.Instance.TryGet(ObjectID, out var identifiable))
        {
            Debug.LogError($"[GameObjectActiveEvent] 유효하지 않은 ObjectID: {ObjectID}");
            return;
        }

        target = identifiable.gameObject;
    }

    public override IEnumerator Execute()
    {
        if (target == null)
        {
            Debug.LogError("[GameObjectActiveEvent] 타겟 오브젝트가 설정되지 않았습니다. Name: " + name);
            yield break;
        }

        target.SetActive(SetActiveTo);
        yield return null;
    }
}
