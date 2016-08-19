using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeManager))]
public class NodeManagerEditor : Editor
{
    NodeManager t;
    SerializedObject GetTarget;

    void OnEnable()
    {
        t = (NodeManager)target;
        GetTarget = new SerializedObject(t);
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

        for (int i = 0; i < t.NodesSize; i++)
        {
            Node node = t.GetNodeAtIndex(i);
            if (node == null) continue;
            GUILayout.BeginHorizontal();
            GUILayout.Label(node.gameObject.name);

            if (GUILayout.Button("X"))
                t.RemoveNodeAt(i);

            GUILayout.EndHorizontal();
        }

        GetTarget.ApplyModifiedProperties();
    }
}