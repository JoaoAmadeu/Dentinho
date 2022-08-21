using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        ReadOnlyAttribute readOnly = attribute as ReadOnlyAttribute;
        label.tooltip = readOnly.tooltip == "" ? "READ ONLY" : "READ ONLY. " + readOnly.tooltip;

        GUI.enabled = false;
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property, label);
        EditorGUI.EndProperty();
        GUI.enabled = true;
    }
}