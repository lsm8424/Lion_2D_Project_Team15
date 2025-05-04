using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerCondition_SO))]
public class TriggerConditionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        TriggerCondition_SO condition = (TriggerCondition_SO)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ObjectID"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ComponentName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ProcessType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ProcessName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ComparisonType"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ValueType"));

        switch (condition.ValueType)
        {
            case TriggerCondition_SO.EValueType.Bool:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BoolValue"));
                break;
            case TriggerCondition_SO.EValueType.Int:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IntValue"));
                break;
            case TriggerCondition_SO.EValueType.Float:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FloatValue"));
                break;
            case TriggerCondition_SO.EValueType.String:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StringValue"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
