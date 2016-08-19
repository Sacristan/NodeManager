using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;
    private int _index;

    private Node _nextNode = null;
    private Node _prevNode = null;

    public Node NextNode
    {
        get
        {
            if (_nextNode == null) _nextNode = _nodeManager.GetNodeAtIndex(_index + 1);
            return _nextNode;
        }
    }

    public Node PreviousNode
    {
        get
        {
            if (_prevNode == null) _prevNode = _nodeManager.GetNodeAtIndex(_index - 1);
            return _prevNode;
        }
    }


    public static Node Create(GameObject nodeTemplate, NodeManager nodeManager)
    {
        GameObject nodeGO = Instantiate(nodeTemplate) as GameObject;

        Node node = nodeGO.GetComponent<Node>();
        node._nodeManager = nodeManager;
        node._index = nodeManager.Nodes.Count;

        node.Setup();
        node.AnchorCurve();

        return node;
    }

    private void AnchorCurve()
    {
        if (PreviousNode == null) return;

        BezierCurve nodeCurve = gameObject.GetComponent<BezierCurve>();
        if (nodeCurve == null) nodeCurve = gameObject.AddComponent<BezierCurve>();

        BezierPoint point1 = nodeCurve.AddPointAt(PreviousNode.transform.position);
        BezierPoint point2 = nodeCurve.AddPointAt(transform.position);

        point1.transform.parent = PreviousNode.transform;
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
