using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    private List<Node> nodes;

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

    public Node GetNodeAtIndex(int index)
    {
        if (Nodes.ElementAtOrDefault(index) == null)
            return null;
        else
            return Nodes[index];
    }

}
