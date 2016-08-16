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
        nodes.Add(CreateNode());
    }

    public void RemoveNodeAt(int index)
    {
        Node node = nodes[index];
        nodes.RemoveAt(index);
        DestroyImmediate(node.gameObject);
    }

    private Node CreateNode()
    {
        GameObject button = Instantiate(buttonTemplate, GetPositionForNode(), Quaternion.identity) as GameObject;
        button.name = "Node" + (nodes.Count+1);
        button.transform.SetParent(transform);
        return button.GetComponent<Node>();
    }

    private Vector3 GetPositionForNode()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        return pos;
    }

}
