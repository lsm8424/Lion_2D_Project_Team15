using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(
    fileName = "SimpleEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/SimpleEventFunction_SO"
)]
public class SimpleEventFunction_SO : EventFunction_SO
{
    public SimpleEventStruct[] SimpleEvents;
    IEnumerator[] Coroutines;

    [Serializable]
    public struct SimpleEventStruct
    {
        public string ObjectId; // ObjectId
        public string ComponentName; // 멤버변수 이름, 메소드 이름, 프로퍼티 이름
        public EProcessType ProcessType;
        public string ProcessName;

        public EValueType ValueType;
        public bool BoolValue;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
        public Vector3 Vector3Value;

        public ParameterInfo[] Parameters;

        [Range(0, 100)]
        public float DelaySeconds;
    }

    public enum EProcessType
    {
        Field,
        Property,
        Method,
    }

    public enum EValueType
    {
        Bool,
        Int,
        Float,
        String,
        Vector3,
    }

    [Serializable]
    public struct ParameterInfo
    {
        public ParameterType Type;
        public string Name;
        public int IntValue;
        public float FloatValue;
        public bool BoolValue;
        public string StringValue;
        public Vector3 Vector3Value;
    }

    public enum ParameterType
    {
        Int,
        Bool,
        Float,
        String,
        Vector3,
    }

    public SimpleEventFunction_SO()
    {
        FunctionType = EGameEventFunctionType.Normal;
    }

    public override IEnumerator Execute()
    {
        EventFunctionTracker.BeginEvent();
        SimpleEventStruct[] events = SimpleEvents;

        for (int i = 0; i < events.Length; ++i)
        {
            if (events[i].DelaySeconds > 0f)
                yield return new WaitForSeconds(events[i].DelaySeconds);

            yield return Coroutines[i];
        }
        EventFunctionTracker.EndEvent();
    }

    /// <summary>
    /// 리플렉션을 이용한 세팅
    /// </summary>
    public override void Setup()
    {
        Coroutines = new IEnumerator[SimpleEvents.Length];
        var idManager = IDManager.Instance.Identifiers;

        for (int i = 0; i < SimpleEvents.Length; ++i)
        {
            var simpleEvent = SimpleEvents[i];

            if (!idManager.TryGetValue(simpleEvent.ObjectId, out var objRef))
            {
                Debug.LogError($"[SimpleEventFunction] 잘못된 TargetId입니다: {simpleEvent.ObjectId}");
                continue;
            }

            var go = objRef.gameObject;
            var componentType = Type.GetType(simpleEvent.ComponentName);
            if (componentType == null)
            {
                Debug.LogError(
                    $"[SimpleEventFunction] 잘못된 ComponentType입니다: {simpleEvent.ComponentName}"
                );
                continue;
            }

            var component = go.GetComponent(componentType);
            if (component == null)
            {
                Debug.LogError(
                    $"[SimpleEventFunction] Component를 찾을 수 없습니다. Type: {simpleEvent.ComponentName} in {go.name}"
                );
                continue;
            }

            GenerateCoroutine(simpleEvent, component, componentType, out Coroutines[i]);
        }
    }

    void GenerateCoroutine(
        SimpleEventStruct evt,
        object component,
        Type componentType,
        out IEnumerator coroutine
    )
    {
        coroutine = null;

        switch (evt.ProcessType)
        {
            case EProcessType.Field:
                var fieldInfo = componentType.GetField(
                    evt.ProcessName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );
                if (fieldInfo == null)
                {
                    Debug.LogError($"[SimpleEventFunction] Field 미발견: {evt.ProcessName}");
                    return;
                }
                var value = GetValueByFieldType(evt);
                coroutine = FieldSetterCoroutine(fieldInfo, component, value);
                break;
            case EProcessType.Property:
                var propInfo = componentType.GetProperty(
                    evt.ProcessName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );
                if (propInfo == null)
                {
                    Debug.LogError($"[SimpleEventFunction] Property 미발견: {evt.ProcessName}");
                    return;
                }

                value = GetValueByFieldType(evt);
                coroutine = PropertySetterCoroutine(propInfo, component, value);
                break;
            case EProcessType.Method:
                var methodInfo = componentType.GetMethod(
                    evt.ProcessName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );
                if (methodInfo == null)
                {
                    Debug.LogError($"[SimpleEventFunction] Method 미발견: {evt.ProcessName}");
                    return;
                }

                var parameters = ConvertParameter(evt.Parameters);
                coroutine = MethodInvokerCoroutine(methodInfo, component, parameters);
                break;
            default:
                Debug.LogError($"유효한 타입이 아닙니다. {evt.ProcessType}");
                break;
        }

        if (coroutine == null)
            Debug.LogError("Coroutine이 실행되지 않았습니다.");
    }

    IEnumerator FieldSetterCoroutine(FieldInfo fieldInfo, object component, object value)
    {
        if (component == null || fieldInfo == null || value == null)
        {
            Debug.LogError("FieldSetter 오류");
            yield break;
        }
        fieldInfo.SetValue(component, value);
    }

    IEnumerator PropertySetterCoroutine(PropertyInfo propInfo, object component, object value)
    {
        if (propInfo == null || component == null || value == null)
        {
            Debug.LogError("PropertySetter 오류");
            yield break;
        }
        propInfo.SetValue(component, value);
    }

    IEnumerator MethodInvokerCoroutine(MethodInfo methodInfo, object component, object[] parameters)
    {
        if (methodInfo == null || component == null)
        {
            Debug.LogError("MethodInvoker 오류");
            yield break;
        }

        if (parameters == null || parameters.Length == 0)
            methodInfo.Invoke(component, null);
        else
            methodInfo.Invoke(component, parameters);
    }

    object[] ConvertParameter(ParameterInfo[] parameters)
    {
        if (parameters.Length == 0)
            return null;

        object[] convertedParameters = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; ++i)
        {
            try
            {
                convertedParameters[i] = parameters[i].Type switch
                {
                    ParameterType.Int => parameters[i].IntValue,
                    ParameterType.Bool => parameters[i].BoolValue,
                    ParameterType.Float => parameters[i].FloatValue,
                    ParameterType.String => parameters[i].StringValue,
                    ParameterType.Vector3 => parameters[i].Vector3Value,
                };
            }
            catch (Exception e)
            {
                Debug.LogError($"타입변환 에러 발생 {parameters[i].Name} {e.Message}");
            }
        }

        return convertedParameters;
    }

    object GetValueByFieldType(SimpleEventStruct simpleEvent)
    {
        return simpleEvent.ValueType switch
        {
            EValueType.Bool => simpleEvent.BoolValue,
            EValueType.Int => simpleEvent.IntValue,
            EValueType.Float => simpleEvent.FloatValue,
            EValueType.String => simpleEvent.StringValue,
            EValueType.Vector3 => simpleEvent.Vector3Value,
            _ => null
        };
    }
}
