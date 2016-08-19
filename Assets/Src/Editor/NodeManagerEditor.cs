using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(NodeManager))]
public class NodeManagerEditor : Editor
{
    NodeManager t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;

    void OnEnable()
    {
        t = (NodeManager)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("nodes");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GetTarget.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Node"))
        {
            t.AddNode();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            if (t.Nodes.ElementAtOrDefault(i) == null) continue;

            Node node = t.Nodes[i];

            GUILayout.BeginHorizontal();
            GUILayout.Label(node.gameObject.name);

            if (GUILayout.Button("X"))
                t.RemoveNodeAt(i);

            GUILayout.EndHorizontal();
        }

        GetTarget.ApplyModifiedProperties();
    }
}