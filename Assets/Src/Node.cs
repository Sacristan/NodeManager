using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;
    private int _index;

    public static Node Create(GameObject nodeTemplate, NodeManager nodeManager)
    {
        GameObject nodeGO = Instantiate(nodeTemplate) as GameObject;

        Node node = nodeGO.GetComponent<Node>();
        node._nodeManager = nodeManager;
        node._index = nodeManager.Nodes.Count;

        node.Setup();

        return nodeGO.GetComponent<Node>();
    }

    private void Setup()
    {
        transform.position = GetPosition();
        gameObject.name = "Node" + _index;
        transform.SetParent(_nodeManager.transform);
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = _nodeManager.transform.position;
        return pos;
    }

}
