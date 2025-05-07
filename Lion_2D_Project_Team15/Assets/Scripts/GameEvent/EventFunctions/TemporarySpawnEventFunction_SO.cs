using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(
    fileName = "TemporarySpawnEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/TemporarySpawnEventFunction_SO"
)]
public class TemporarySpawnEventFunction_SO : EventFunction_SO
{
    public GameObject Prefab;
    public string ObjectID;
    public Vector3 Offset;
    public float Duration;

    GameObject _spawnedObject;
    float _prevAnimationSpeed;

    float _timer;

    public TemporarySpawnEventFunction_SO()
    {
        FunctionType = EGameEventFunctionType.ParameterFunction;
    }

    public override IEnumerator Execute()
    {
        _timer = 0f;
        IDManager.Instance.TryGet(ObjectID, out var targetObject);
        var parent = targetObject.transform;
        var go = GameManager.Instantiate(Prefab, parent.position + Offset, Quaternion.identity);
        go.transform.SetParent(parent);

        while (_timer < Duration)
        {
            _timer += Time.deltaTime * GameManager.Instance.DialogueTimeScale;

            if (GameManager.Instance.ShouldWaitForDialogue())
            {
                OnPause();
                yield return new WaitUntil(() => !GameManager.Instance.ShouldWaitForDialogue());
                OnReload();
            }

            yield return null;
        }

        Destroy(go);
    }

    public void OnPause()
    {
        if (_spawnedObject == null)
            return;

        if (_spawnedObject.TryGetComponent<Animator>(out var animator))
        {
            return;
        }

        _prevAnimationSpeed = animator.speed;
        animator.speed = 0;
    }

    public void OnReload()
    {
        if (_spawnedObject == null)
            return;

        if (_spawnedObject.TryGetComponent<Animator>(out var animator))
        {
            return;
        }

        animator.speed = _prevAnimationSpeed;
    }

    public override void Setup() { }
}
