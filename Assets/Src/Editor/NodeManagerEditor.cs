using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NodeManager))]
public class NodeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NodeManager nodeManager = (NodeManager) target;
        if (GUILayout.Button("Create Button"))
        {
            nodeManager.AddNew();
        }
    }
}