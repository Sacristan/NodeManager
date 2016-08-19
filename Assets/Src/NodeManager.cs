using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    private Node[] nodes;

    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private GameObject knobTemplate;

    public Node[] Nodes { get { return nodes; } }
    public GameObject KnobTemplate { get { return knobTemplate; } }

    public void AddNode()
    {
        List<Node> tmpList = new List<Node>(nodes);
        Node node = Node.Create(buttonTemplate, this);
        tmpList.Add(node);
        nodes = tmpList.ToArray();
        node.Setup();
    }

    public void RemoveNodeAt(int index)
    {
        Node node = GetNodeAtIndex(index);
        if (node != null)
        {
            List<Node> tmpList = new List<Node>(nodes);
            tmpList.RemoveAt(index);
            nodes = tmpList.ToArray();
            DestroyImmediate(node.gameObject);
        }
    }

    public Node GetNodeAtIndex(int index)
    {
        if (index < 0 || Nodes.Length < index)
            return null;
        else
            return Nodes[index];
    }

    public int IndexForNode(Node node)
    {
        int result = -1;

        for(int i=0;i<Nodes.Length;i++)
        {
            if (Nodes[i].Equals(node))
            {
                result = i;
                break;
            } 
        }

        return result;
    }

}
