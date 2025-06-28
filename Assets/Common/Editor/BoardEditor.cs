
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create"))
        {
            ((Board)target).Create();
        }
        if (GUILayout.Button("Clear"))
        {
            ((Board)target).Clear();
        }
    }
}