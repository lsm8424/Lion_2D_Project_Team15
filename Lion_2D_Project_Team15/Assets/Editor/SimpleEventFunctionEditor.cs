using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(SimpleEventFunction))]
public class SimpleEventFunctionEditor : Editor
{
    private SerializedProperty simpleEventsProp;

    private void OnEnable()
    {
        simpleEventsProp = serializedObject.FindProperty("SimpleEvents");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Simple Events", EditorStyles.boldLabel);

        for (int i = 0; i < simpleEventsProp.arraySize; i++)
        {
            var element = simpleEventsProp.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.PropertyField(element.FindPropertyRelative("TargetId"));
            EditorGUILayout.PropertyField(element.FindPropertyRelative("DelaySeconds"));

            SerializedProperty componentNameProp = element.FindPropertyRelative("ComponentName");
            SerializedProperty NameToProcessProp = element.FindPropertyRelative("NameToProcess");
            SerializedProperty processTypeProp = element.FindPropertyRelative("ProcessType");
            SerializedProperty fieldTypeProp = element.FindPropertyRelative("FieldType");

            EditorGUILayout.PropertyField(componentNameProp);
            EditorGUILayout.PropertyField(processTypeProp);
            EditorGUILayout.PropertyField(NameToProcessProp);

            EditorGUILayout.PropertyField(fieldTypeProp);

            // --- FieldType에 따라 Value 입력창 제어 ---
            SimpleEventFunction.EFieldType fieldType = (SimpleEventFunction.EFieldType)fieldTypeProp.enumValueIndex;

            switch (fieldType)
            {
                case SimpleEventFunction.EFieldType.Bool:
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("BoolValue"), new GUIContent("Bool Value"));
                    break;
                case SimpleEventFunction.EFieldType.Int:
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("IntValue"), new GUIContent("Int Value"));
                    break;
                case SimpleEventFunction.EFieldType.Float:
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("FloatValue"), new GUIContent("Float Value"));
                    break;
                case SimpleEventFunction.EFieldType.String:
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("StringValue"), new GUIContent("String Value"));
                    break;
                case SimpleEventFunction.EFieldType.Vector3:
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("Vector3Value"), new GUIContent("Vector3 Value"));
                    break;
            }

            if (GUILayout.Button("삭제"))
            {
                simpleEventsProp.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("새 이벤트 추가"))
        {
            simpleEventsProp.InsertArrayElementAtIndex(simpleEventsProp.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
