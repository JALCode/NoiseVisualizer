using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ValueButton))]
public class ValueButtonEditor : Editor {

    public override void OnInspectorGUI() {
        ValueButton button = (ValueButton)target;

        button.baseValue = EditorGUILayout.FloatField("Base Value", button.baseValue);
        button.baseIncrement = EditorGUILayout.FloatField("Base Increment", button.baseIncrement);
        button.isInteger = EditorGUILayout.Toggle("Is Integer", button.isInteger);
        button.timeTrigger = EditorGUILayout.FloatField("Time to Trigger", button.timeTrigger);

        DrawDefaultInspector();
    }

    
}