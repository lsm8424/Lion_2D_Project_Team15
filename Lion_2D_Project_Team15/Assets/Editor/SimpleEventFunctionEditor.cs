using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleEventFunction_SO))]
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
            SerializedProperty processNameProp = element.FindPropertyRelative("ProcessName");
            SerializedProperty processTypeProp = element.FindPropertyRelative("ProcessType");
            SerializedProperty valueTypeProp = element.FindPropertyRelative("ValueType");

            EditorGUILayout.PropertyField(componentNameProp);
            EditorGUILayout.PropertyField(processTypeProp);
            EditorGUILayout.PropertyField(processNameProp);

            SimpleEventFunction_SO.EProcessType processType = (SimpleEventFunction_SO.EProcessType)processTypeProp.enumValueIndex;

            // 처리타입이 멤버변수, 프로퍼티 라면 타입과 그에 해당하는 입력칸 표시
            if (processType == SimpleEventFunction_SO.EProcessType.Field || processType == SimpleEventFunction_SO.EProcessType.Property)
            {
                EditorGUILayout.PropertyField(valueTypeProp);

                // --- ValueType에 따라 Value 입력창 제어 ---
                SimpleEventFunction_SO.EValueType fieldType = (SimpleEventFunction_SO.EValueType)valueTypeProp.enumValueIndex;
                SetFieldMember(fieldType, element);
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
}
