using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TemporarySpawnEventFunction_SO", menuName = "Scriptable Objects/EventFunction/TemporarySpawnEventFunction_SO")]
public class TemporarySpawnEventFunction_SO : EventFunction_SO
{
    public GameObject Prefab;
    public Transform Parent;
    public Vector3 Offset;
    public float Duration;

    public TemporarySpawnEventFunction_SO()
    {
        FunctionType = EGameEventFunctionType.ParameterFunction;
    }

    public override IEnumerator Execute()
    {
        var go = GameManager.Instantiate(Prefab, Parent.position + Offset, Quaternion.identity);
        go.transform.SetParent(Parent);
        yield return new WaitForSeconds(Duration);
        Destroy(go);
    }

    public override void Setup() { }
}
