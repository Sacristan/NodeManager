using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    private List<Node> nodes;

    [SerializeField]
    private GameObject buttonTemplate;

    public int NodesSize { get { return nodes.Count; } }

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
        Node node = null;
        if (index >= 0 && nodes.Count > index) node = nodes[index];
        Debug.Log("Index: "+index+" Node: "+node);
        return node;
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
