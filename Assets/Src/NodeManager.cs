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

    private static NodeManager instance;

    public static GameObject KnobTemplate { get { return instance.knobTemplate; } }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

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
        //Debug.Log("Index: "+index+" Node: "+node);
        return node;
    }

    public int IndexForNode(Node node)
    {
        int result = -1;
        result = nodes.IndexOf(node);
        return result;
    }

}
