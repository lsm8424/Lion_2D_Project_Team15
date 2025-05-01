using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "ParameterEventFunction_SO", menuName = "Scriptable Objects/ParameterEventFunction_SO")]
public class ParameterEventFunction_SO : EventFunction_SO
{
    public FunctionInfo[] Functions;
    Action[] _actions;

    [Serializable]
    public struct FunctionInfo
    {
        public string ObjectID;
        public string FunctionName;
        public ParameterInfo[] Parameters;
        [Range(0, 100f)] public float Delay;

        public void Deconstruct(out string objectID, out string functionName, out ParameterInfo[] parameters)
        {
            objectID = ObjectID;
            functionName = FunctionName;
            parameters = Parameters;
        }
    }

    [Serializable]
    public struct ParameterInfo
    {
        public ParameterType Type;
        public string Name;
        public string Value;
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

    public ParameterEventFunction_SO()
    {
        FunctionType = EGameEventFunctionType.ParameterFunction;
    }

    public override void Setup()
    {

        foreach (var function in Functions)
        {
            var (objectID, functionName, parameters) = function;

            if (!IDManager.Instance.TryGet(objectID, out var targetObject))
                return;

            var componentType = Type.GetType(functionName);
            if (componentType == null)
            {
                Debug.LogError($"타입 오류${functionName}");
                return;
            }

            var component = targetObject.GetComponent(componentType);
            if (component == null)
            {
                Debug.LogError($"컴포넌트 미발견 ${componentType}");
                return;
            }

            MethodInfo methodInfo = componentType.GetMethod(functionName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                Debug.LogError($"메소드 미발견 ${functionName}");
                return;
            }

            object[] convertedParameters = ConvertParameter(parameters);
            _actions[0] = () => methodInfo.Invoke(component, convertedParameters);
        }
    }

    object[] ConvertParameter(ParameterInfo[] parameters)
    {
        object[] convertedParameters = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; ++i)
        {
            try
            {
                convertedParameters[i] = parameters[i].Type switch
                {
                    ParameterType.Int => int.Parse(parameters[i].Value),
                    ParameterType.Bool => bool.Parse(parameters[i].Value),
                    ParameterType.Float => float.Parse(parameters[i].Value),
                    ParameterType.String => parameters[i].Value,
                    ParameterType.Vector3 => parameters[i].Vector3Value
                };
            } catch (Exception e)
            {
                Debug.LogError($"타입변환 에러 발생 {parameters[i].Name} {e.Message}");
            }
        }

        return convertedParameters;
    }

    public override IEnumerator Execute()
    {
        for (int i = 0; i < Functions.Length; ++i)
        {
            var function = Functions[i];

            if (function.Delay > 0)
                yield return new WaitForSeconds(function.Delay);

            _actions[i]?.Invoke();
        }
    }
}
