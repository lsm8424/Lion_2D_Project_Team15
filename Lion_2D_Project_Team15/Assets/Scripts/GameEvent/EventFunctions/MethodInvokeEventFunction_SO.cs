using System.Collections;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(
    fileName = "MethodInvokeEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/MethodInvokeEventFunction_SO"
)]
public class MethodInvokeEventFunction_SO : EventFunction_SO
{
    [Header("오브젝트 이름 (씬 내 GameObject 이름)")]
    public string targetObjectName;
    [Header("컴포넌트(스크립트) 이름 (예: PlayerMovement)")]
    public string componentTypeName;
    [Header("실행할 함수 이름 (예: MyMethod)")]
    public string methodName;

    public override void Setup() { }

    public override IEnumerator Execute()
    {
        if (string.IsNullOrEmpty(targetObjectName) || string.IsNullOrEmpty(componentTypeName) || string.IsNullOrEmpty(methodName))
        {
            Debug.LogError("[MethodInvokeEventFunction_SO] 필드가 비어있습니다.");
            yield break;
        }

        GameObject target = GameObject.Find(targetObjectName);
        if (target == null)
        {
            Debug.LogError($"[MethodInvokeEventFunction_SO] 오브젝트를 찾을 수 없습니다: {targetObjectName}");
            yield break;
        }

        var component = target.GetComponent(componentTypeName);
        if (component == null)
        {
            Debug.LogError($"[MethodInvokeEventFunction_SO] 컴포넌트를 찾을 수 없습니다: {componentTypeName}");
            yield break;
        }

        MethodInfo method = component.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (method == null)
        {
            Debug.LogError($"[MethodInvokeEventFunction_SO] 메서드를 찾을 수 없습니다: {methodName}");
            yield break;
        }

        method.Invoke(component, null);
        Debug.Log($"[MethodInvokeEventFunction_SO] {targetObjectName}의 {componentTypeName}.{methodName}() 실행 완료");
        yield return null;
    }
}
