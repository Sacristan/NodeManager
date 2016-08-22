using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    private List<Node> nodes;

    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private GameObject knobTemplate;

    public int NodesSize { get { return nodes.Count; } }
    public GameObject KnobTemplate { get { return knobTemplate; } }

    public void AddNode()
    {
        Node node = Node.Create(buttonTemplate, this);
        nodes.Add(node);
        node.Setup();
    }

    public void RemoveNodeAt(int index)
    {
        Node node = GetNodeAtIndex(index);
        if (node != null)
        {
            node.TransferAnchors();
            nodes.Remove(node);
            DestroyImmediate(node.gameObject);
        }
    }

    public Node GetNodeAtIndex(int index)
    {
        if (index < 0 || nodes.Count <= index)
            return null;
        else
            return nodes[index];
    }

    public int IndexForNode(Node node)
    {
        int result = -1;

        for(int i=0;i< nodes.Count; i++)
        {
            if (nodes[i].Equals(node))
            {
                result = i;
                break;
            } 
        }

        return result;
    }

}
