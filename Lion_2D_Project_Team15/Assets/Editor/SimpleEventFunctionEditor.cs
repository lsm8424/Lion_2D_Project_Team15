using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleEventFunction_SO))]
public class SimpleEventFunctionEditor : Editor
{
    private SerializedProperty simpleEventsProp;
    private List<bool> parameterFoldouts = new List<bool>();

    private void OnEnable()
    {
        simpleEventsProp = serializedObject.FindProperty("SimpleEvents");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Simple Events", EditorStyles.boldLabel);

        while (parameterFoldouts.Count < simpleEventsProp.arraySize)
            parameterFoldouts.Add(true); // 기본값 true = 열림

        for (int i = 0; i < simpleEventsProp.arraySize; i++)
        {
            var element = simpleEventsProp.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");

            // 🔢 이벤트 번호 라벨
            EditorGUILayout.LabelField($"▶ 이벤트 {i + 1}", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(element.FindPropertyRelative("ObjectId"));
            EditorGUILayout.PropertyField(element.FindPropertyRelative("DelaySeconds"));

            SerializedProperty componentNameProp = element.FindPropertyRelative("ComponentName");
            SerializedProperty processNameProp = element.FindPropertyRelative("ProcessName");
            SerializedProperty processTypeProp = element.FindPropertyRelative("ProcessType");
            SerializedProperty valueTypeProp = element.FindPropertyRelative("ValueType");

            EditorGUILayout.PropertyField(componentNameProp);
            EditorGUILayout.PropertyField(processTypeProp);
            EditorGUILayout.PropertyField(processNameProp);

            var processType = (SimpleEventFunction_SO.EProcessType)processTypeProp.enumValueIndex;

            if (processType == SimpleEventFunction_SO.EProcessType.Field || processType == SimpleEventFunction_SO.EProcessType.Property)
            {
                EditorGUILayout.PropertyField(valueTypeProp);
                var fieldType = (SimpleEventFunction_SO.EValueType)valueTypeProp.enumValueIndex;
                SetFieldMember(fieldType, element);
            }
            else if (processType == SimpleEventFunction_SO.EProcessType.Method)
            {
                parameterFoldouts[i] = EditorGUILayout.Foldout(parameterFoldouts[i], "매개변수", true);
                if (parameterFoldouts[i])
                {
                    ShowParameterFunctionGUI(element);
                }
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("이벤트 삭제", GUILayout.Width(100)))
            {
                simpleEventsProp.DeleteArrayElementAtIndex(i);
                parameterFoldouts.RemoveAt(i); // 상태도 같이 제거
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("새 이벤트 추가"))
        {
            simpleEventsProp.InsertArrayElementAtIndex(simpleEventsProp.arraySize);
            parameterFoldouts.Add(true); // 새 항목도 열림 상태로
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void ShowParameterFunctionGUI(SerializedProperty simpleEvent)
    {
        SerializedProperty parametersProp = simpleEvent.FindPropertyRelative("Parameters");

        EditorGUILayout.LabelField("매개변수", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");

        for (int i = 0; i < parametersProp.arraySize; ++i)
        {
            var element = parametersProp.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");
            SerializedProperty typeProp = element.FindPropertyRelative("Type");
            EditorGUILayout.PropertyField(typeProp, new GUIContent($"Parameter {i + 1}"));

            SimpleEventFunction_SO.ParameterType parameterType = (SimpleEventFunction_SO.ParameterType)typeProp.enumValueIndex;
            SetParameter(parameterType, element);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("매개변수 삭제", GUILayout.Width(100)))
            {
                parametersProp.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("매개변수 추가", GUILayout.Width(120)))
        {
            parametersProp.InsertArrayElementAtIndex(parametersProp.arraySize);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }


    public void SetFieldMember(SimpleEventFunction_SO.EValueType fieldType, SerializedProperty element)
    {
        switch (fieldType)
        {
            case SimpleEventFunction_SO.EValueType.Bool:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("BoolValue"), new GUIContent("Bool Value"));
                break;
            case SimpleEventFunction_SO.EValueType.Int:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("IntValue"), new GUIContent("Int Value"));
                break;
            case SimpleEventFunction_SO.EValueType.Float:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("FloatValue"), new GUIContent("Float Value"));
                break;
            case SimpleEventFunction_SO.EValueType.String:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("StringValue"), new GUIContent("String Value"));
                break;
            case SimpleEventFunction_SO.EValueType.Vector3:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("Vector3Value"), new GUIContent("Vector3 Value"));
                break;
        }
    }

    public void SetParameter(SimpleEventFunction_SO.ParameterType parameterType, SerializedProperty element)
    {
        switch (parameterType)
        {
            case SimpleEventFunction_SO.ParameterType.Int:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("IntValue"), new GUIContent("Int Value"));
                break;
            case SimpleEventFunction_SO.ParameterType.Bool:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("BoolValue"), new GUIContent("Bool Value"));
                break;
            case SimpleEventFunction_SO.ParameterType.Float:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("FloatValue"), new GUIContent("Float Value"));
                break;
            case SimpleEventFunction_SO.ParameterType.String:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("StringValue"), new GUIContent("String Value"));
                break;
            case SimpleEventFunction_SO.ParameterType.Vector3:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("Vector3Value"), new GUIContent("Vector3 Value"));
                break;
        }
    }
}
