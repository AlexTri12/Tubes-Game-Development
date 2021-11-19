using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor
{
    public BoardCreator current
    {
        get
        {
            return (BoardCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        DrawDefaultInspector();

        if (GUILayout.Button("Clear"))
            current.Clear();
        if (GUILayout.Button("Grow"))
            current.Grow();
        if (GUILayout.Button("Shrink"))
            current.Shrink();
        if (GUILayout.Button("Grow Area Random"))
            current.GrowAreaRandom();
        if (GUILayout.Button("Shrink Area Random"))
            current.ShrinkAreaRandom();
        if (GUILayout.Button("Grow Area All"))
            current.GrowAreaAll();
        if (GUILayout.Button("Shrink Area All"))
            current.ShrinkAreaAll();
        if (GUILayout.Button("Grow Area Rect"))
            current.GrowAreaRect();
        if (GUILayout.Button("Shrink Area Rect"))
            current.ShrinkAreaRect();
        if (GUILayout.Button("Save"))
            current.Save();
        if (GUILayout.Button("Load"))
            current.Load();

        if (GUI.changed)
            current.UpdateMarker();
    }
}
