using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    private List<Node> nodes = new List<Node>(1);

    [SerializeField]
    private GameObject buttonTemplate;

    public List<Node> Nodes { get { return nodes; } }

    public void AddNode()
    {
        nodes.Add(Node.Create(buttonTemplate, this));
    }

    public void RemoveNodeAt(int index)
    {
        Node node = nodes[index];
        nodes.RemoveAt(index);
        DestroyImmediate(node.gameObject);
    }

}
