using UnityEditor;
using UnityEngine;

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
            SerializedProperty processNameProp = element.FindPropertyRelative("ProcessName");
            SerializedProperty processTypeProp = element.FindPropertyRelative("ProcessType");
            SerializedProperty valueTypeProp = element.FindPropertyRelative("ValueType");

            EditorGUILayout.PropertyField(componentNameProp);
            EditorGUILayout.PropertyField(processTypeProp);
            EditorGUILayout.PropertyField(processNameProp);

            SimpleEventFunction.EProcessType processType = (SimpleEventFunction.EProcessType)processTypeProp.enumValueIndex;

            // ó��Ÿ���� �������, ������Ƽ ��� Ÿ�԰� �׿� �ش��ϴ� �Է�ĭ ǥ��
            if (processType == SimpleEventFunction.EProcessType.Field || processType == SimpleEventFunction.EProcessType.Property)
            {
                EditorGUILayout.PropertyField(valueTypeProp);

                // --- ValueType�� ���� Value �Է�â ���� ---
                SimpleEventFunction.EValueType fieldType = (SimpleEventFunction.EValueType)valueTypeProp.enumValueIndex;
                SetFieldMember(fieldType, element);
            }

            if (GUILayout.Button("����"))
            {
                simpleEventsProp.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("�� �̺�Ʈ �߰�"))
        {
            simpleEventsProp.InsertArrayElementAtIndex(simpleEventsProp.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void SetFieldMember(SimpleEventFunction.EValueType fieldType, SerializedProperty element)
    {
        switch (fieldType)
        {
            case SimpleEventFunction.EValueType.Bool:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("BoolValue"), new GUIContent("Bool Value"));
                break;
            case SimpleEventFunction.EValueType.Int:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("IntValue"), new GUIContent("Int Value"));
                break;
            case SimpleEventFunction.EValueType.Float:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("FloatValue"), new GUIContent("Float Value"));
                break;
            case SimpleEventFunction.EValueType.String:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("StringValue"), new GUIContent("String Value"));
                break;
            case SimpleEventFunction.EValueType.Vector3:
                EditorGUILayout.PropertyField(element.FindPropertyRelative("Vector3Value"), new GUIContent("Vector3 Value"));
                break;
        }
    }
}
