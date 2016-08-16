using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

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
        GetTarget.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add New"))
        {
            t.AddNode();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            //SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("Node{0} {1}", i, t.Nodes[i].gameObject.name));
            if (GUILayout.Button("X"))
            {
                t.RemoveNodeAt(i);
            }
            GUILayout.EndHorizontal();
        }

        GetTarget.ApplyModifiedProperties();
    }
}