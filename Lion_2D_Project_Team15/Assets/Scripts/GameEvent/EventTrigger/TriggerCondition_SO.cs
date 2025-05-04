using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "TriggerCondition_SO", menuName = "Scriptable Objects/TriggerCondition_SO")]
public class TriggerCondition_SO : ScriptableObject
{
    public string ObjectID;
    public ECompareMethod ComparisonType;

    public string ComponentName; // 멤버변수 이름, 메소드 이름, 프로퍼티 이름
    public EComparableType ProcessType;
    public string ProcessName;

    public EValueType ValueType;
    public bool BoolValue;
    public int IntValue;
    public float FloatValue;
    public string StringValue;

    Func<object> _operand;

    public void SetUp()
    {
        if (!IDManager.Instance.TryGet(ObjectID, out var targetObj))
        {
            Debug.LogError("유효하지 않은 ObjectID: " + ObjectID);
            return;
        }

        Type componentType = Type.GetType(ComponentName);
        if (componentType == null)
        {
            Debug.LogError("잘못된 ComponentName: " + ComponentName);
            return;
        }
        var component = targetObj.GetComponent(componentType);

        switch (ProcessType)
        {
            case EComparableType.Field:
                var fieldInfo = componentType.GetField(ProcessName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fieldInfo == null)
                {
                    Debug.LogError($"Field 미발견: {ProcessName}");
                    return;
                }
                _operand = () => fieldInfo.GetValue(component);
                break;
            case EComparableType.Property:
                var propInfo = componentType.GetProperty(ProcessName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (propInfo == null)
                {
                    Debug.LogError($"Property 미발견: {ProcessName}");
                    return;
                }
                _operand = () => propInfo.GetValue(component);
                break;
        }
    }

    public bool Validate()
    {
        
        object operand1 = GetValue();
        object operand2 = _operand?.Invoke();

        if (operand2 == null)
        {
            Debug.LogError($"값을 찾을 수 없습니다. {ProcessName}");
            return false;
        }

        try
        {
            bool result = false;
            switch (ValueType)
            {
                case EValueType.Bool:
                    result = ComparisonUtil<bool>.Comparisons[ComparisonType]((bool)operand1, (bool)operand2);
                    break;
                case EValueType.Int:
                    result = ComparisonUtil<int>.Comparisons[ComparisonType]((int)operand1, (int)operand2);
                    break;
                case EValueType.Float:
                    result = ComparisonUtil<float>.Comparisons[ComparisonType]((float)operand1, (float)operand2);
                    break;
                case EValueType.String:
                    result = ComparisonUtil<string>.Comparisons[ComparisonType]((string)operand1, (string)operand2);
                    break;
            }

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("비교연산이 정상적으로 실행되지 않음 " + e.Message);
        }


        return false;
    }

    public object GetValue() => ValueType switch
    {
        EValueType.Bool => BoolValue,
        EValueType.Int => IntValue,
        EValueType.Float => FloatValue,
        EValueType.String => StringValue,
        _ => null
    };

    public enum EValueType
    {
        Bool,
        Int,
        Float,
        String,
    }

    public enum EComparableType
    {
        Field,
        Property,
    }

    public enum ECompareMethod
    {
        Less,
        LessEqual,
        Equal,
        NotEqual,
        GreaterEqual,
        Greater
    }

    private static class ComparisonUtil<T>
    {
        private static readonly Comparer<T> comparer = Comparer<T>.Default;

        public static readonly Dictionary<ECompareMethod, Func<T, T, bool>> Comparisons = new()
        {
            { ECompareMethod.Less, (a, b) => comparer.Compare(a, b) < 0 },
            { ECompareMethod.LessEqual, (a, b) => comparer.Compare(a, b) <= 0 },
            { ECompareMethod.Equal, (a, b) => comparer.Compare(a, b) == 0 },
            { ECompareMethod.NotEqual, (a, b) => comparer.Compare(a, b) != 0 },
            { ECompareMethod.GreaterEqual, (a, b) => comparer.Compare(a, b) >= 0 },
            { ECompareMethod.Greater, (a, b) => comparer.Compare(a, b) > 0 },
        };
    }
}
