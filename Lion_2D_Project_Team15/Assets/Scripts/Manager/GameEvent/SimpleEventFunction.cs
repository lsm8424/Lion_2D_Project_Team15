using System;
using System.Collections;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleEventFunction", menuName = "Scriptable Objects/SimpleEventFunction")]
public class SimpleEventFunction : EventFunctionSO
{

    public SimpleEventStruct[] SimpleEvents;
    Action[] Process;

    [Serializable]
    public struct SimpleEventStruct
    {
        public string TargetId;
        public string ComponentName;
        public EProcessType ProcessType;
        public string NameToProcess;

        public EFieldType FieldType;
        public bool BoolValue;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
        public Vector3 Vector3Value;

        [Range(0, 100)]
        public float DelaySeconds;
    }

    public enum EProcessType
    {
        Field,
        Property,
        Method
    }
    public enum EFieldType
    {
        Bool,
        Int,
        Float,
        String,
        Vector3,
    }


    public SimpleEventFunction()
    {
        FunctionType = EGameEventFunctionType.Normal;
    }
    public override void Execute()
    {
        GameManager.Instance.StartCoroutine(RunEventsCoroutine(SimpleEvents));
    }

    private IEnumerator RunEventsCoroutine(SimpleEventStruct[] events)
    {
        for (int i = 0; i < events.Length; ++i)
        {
            if (events[i].DelaySeconds > 0f)
                yield return new WaitForSeconds(events[i].DelaySeconds);

            Process[i]();
        }
    }

    public override void Initialize()
    {
        Process = new Action[SimpleEvents.Length];
        var warehouse = StageWarehouse.Instance.Warehouse;

        for (int i = 0; i < Process.Length; ++i)
        {
            var simpleEvent = SimpleEvents[i];

            if (!warehouse.ContainsKey(simpleEvent.TargetId))
            {
                Debug.LogError($"[SimpleEventFunction] 잘못된 TargetId입니다: {simpleEvent.TargetId}");
                return;
            }

            GameObject go = warehouse[simpleEvent.TargetId];
            var componentType = Type.GetType(simpleEvent.ComponentName);

            if (componentType == null)
            {
                Debug.LogError($"[SimpleEventFunction] 잘못된 ComponentType입니다: {simpleEvent.ComponentName}");
                return;
            }

            var component = go.GetComponent(componentType);
            if (component == null)
            {
                Debug.LogError($"[SimpleEventFunction] Component를 찾을 수 없습니다. Type: {simpleEvent.ComponentName} in {go.name}");
                return;
            }

            switch (simpleEvent.ProcessType)
            {
                case EProcessType.Field:
                    Process[i] += () => SetFieldValue(component, componentType, simpleEvent);
                    break;
                case EProcessType.Property:
                    Process[i] += () => SetPropertyValue(component, componentType, simpleEvent);
                    break;
                case EProcessType.Method:
                    Process[i] += () => InvokeMethod(component, componentType, simpleEvent);
                    break;
            }
        }
    }

    private void SetFieldValue(object component, Type componentType, SimpleEventStruct simpleEvent)
    {
        FieldInfo fieldInfo = componentType.GetField(simpleEvent.NameToProcess, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo == null)
        {
            Debug.LogError($"[SimpleEventFunction] Field를 찾지 못했습니다: {simpleEvent.NameToProcess}");
            return;
        }

        object value = GetValueByFieldType(simpleEvent);
        fieldInfo.SetValue(component, value);
    }

    private void SetPropertyValue(object component, Type componentType, SimpleEventStruct simpleEvent)
    {
        PropertyInfo propInfo = componentType.GetProperty(simpleEvent.NameToProcess, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (propInfo == null)
        {
            Debug.LogError($"[SimpleEventFunction] Property를 찾지 못했습니다: {simpleEvent.NameToProcess}");
            return;
        }

        object value = GetValueByFieldType(simpleEvent);
        propInfo.SetValue(component, value);
    }

    private void InvokeMethod(object component, Type componentType, SimpleEventStruct simpleEvent)
    {
        MethodInfo methodInfo = componentType.GetMethod(simpleEvent.NameToProcess, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo == null)
        {
            Debug.LogError($"[SimpleEventFunction] Method를 찾지 못했습니다: {simpleEvent.NameToProcess}");
            return;
        }

        methodInfo.Invoke(component, null);
    }

    private object GetValueByFieldType(SimpleEventStruct simpleEvent)
    {
        return simpleEvent.FieldType switch
        {
            EFieldType.Bool => simpleEvent.BoolValue,
            EFieldType.Int => simpleEvent.IntValue,
            EFieldType.Float => simpleEvent.FloatValue,
            EFieldType.String => simpleEvent.StringValue,
            EFieldType.Vector3 => simpleEvent.Vector3Value,
            _ => null
        };
    }

}

