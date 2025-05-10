using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ComponentChangeFunction_SO",
    menuName = "Scriptable Objects/EventFunction/ComponentChangeFunction_SO"
)]
public class ComponentChangeFunction_SO : EventFunction_SO
{
    public string ObjectID;
    public string ComponentName; // 예: UnityEngine.SpriteRenderer
    public string MemberName; // 수정할 필드 또는 프로퍼티 이름
    public MemberType TargetMemberType;
    public ValueType TargetValueType;

    public bool BoolValue;
    public int IntValue;
    public float FloatValue;
    public string StringValue;
    public Color ColorValue;
    public Vector3 Vector3Value;
    public UnityEngine.Object ObjectValue;

    public enum MemberType
    {
        Field,
        Property
    }

    public enum ValueType
    {
        Bool,
        Int,
        Float,
        String,
        Color,
        Vector3,
        Object
    }

    public override void Setup() { }

    public override IEnumerator Execute()
    {
        EventFunctionTracker.BeginEvent();
        if (!IDManager.Instance.TryGet(ObjectID, out var targetObj))
        {
            Debug.LogError($"[ComponentChangeFunction] 유효하지 않은 ObjectID: {ObjectID}");
            yield break;
        }

        Type componentType = Type.GetType(ComponentName);
        if (componentType == null)
        {
            componentType = typeof(Collider2D).Assembly.GetType(ComponentName); // Physics2DModule fallback
        }

        if (componentType == null)
        {
            Debug.LogError($"[ComponentChangeFunction] ComponentType을 찾을 수 없습니다: {ComponentName}");
            yield break;
        }

        Component component = targetObj.GetComponent(componentType);
        if (component == null)
        {
            Debug.LogError(
                $"[ComponentChangeFunction] {ComponentName} 컴포넌트가 {targetObj.name}에 없습니다."
            );
            yield break;
        }

        object value = GetTargetValue();

        try
        {
            if (TargetMemberType == MemberType.Property)
            {
                PropertyInfo prop = componentType.GetProperty(
                    MemberName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );
                if (prop == null)
                {
                    Debug.LogError(
                        $"[ComponentChangeFunction] Property '{MemberName}'를 찾을 수 없습니다."
                    );
                    yield break;
                }
                prop.SetValue(component, value);
            }
            else if (TargetMemberType == MemberType.Field)
            {
                FieldInfo field = componentType.GetField(
                    MemberName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );
                if (field == null)
                {
                    Debug.LogError($"[ComponentChangeFunction] Field '{MemberName}'를 찾을 수 없습니다.");
                    yield break;
                }
                field.SetValue(component, value);
            }

            Debug.Log($"[ComponentChangeFunction] {ComponentName}.{MemberName} → {value}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[ComponentChangeFunction] 값 변경 실패: {e.Message}");
        }

        yield return null;
        EventFunctionTracker.EndEvent();
    }

    private object GetTargetValue()
    {
        return TargetValueType switch
        {
            ValueType.Bool => BoolValue,
            ValueType.Int => IntValue,
            ValueType.Float => FloatValue,
            ValueType.String => StringValue,
            ValueType.Color => ColorValue,
            ValueType.Vector3 => Vector3Value,
            ValueType.Object => ObjectValue,
            _ => null
        };
    }
}
