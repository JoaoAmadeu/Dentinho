using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (TitleAttribute))]
public class TitlePropertyDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        //return base.GetHeight () + EditorGUIUtility.singleLineHeight;
        return EditorGUIUtility.singleLineHeight + 4;
    }

    public override void OnGUI (Rect position)
    {
        TitleAttribute header = attribute as TitleAttribute;
        float line = EditorGUIUtility.singleLineHeight;
        Rect labelPosition = new Rect(position.x, position.y + (line), position.width, position.height - 4);
        Rect boxPosition = new Rect(position.x, position.y + (line) + 2 - 3, position.width, position.height - 5);
        Rect linePosition = new Rect(position.x, position.y + (line * 2) - 3, position.width, 2);

        labelPosition.y -= line - 4;
        boxPosition.y -= line - 4;
        linePosition.y -= line - 4;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontStyle = FontStyle.Bold;
        style.fontSize += 1;
        GUI.color = new Color(1, 1, 1, 0.7f);
        GUI.Box(boxPosition, "");
        GUI.color = Color.white;

        EditorGUI.LabelField(labelPosition, header.name, style);
        EditorGUI.DrawRect(linePosition, new Color(1, 1, 1, 0.08f));
    }
}