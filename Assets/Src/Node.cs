using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;

    private Node _nextNode = null;
    private Node _prevNode = null;

    private int _index;

    public Node NextNode
    {
        get
        {
            if (_nextNode == null) _nextNode = _nodeManager.GetNodeAtIndex(Index + 1);
            return _nextNode;
        }
    }

    public Node PreviousNode
    {
        get
        {
            if (_prevNode == null) _prevNode = _nodeManager.GetNodeAtIndex(Index - 1);
            return _prevNode;
        }
    }

    private int Index
    {
        get
        {
            _index = _nodeManager.IndexForNode(this);
            return _index;
        }
    }

    public static Node Create(GameObject nodeTemplate, NodeManager nodeManager)
    {
        GameObject nodeGO = Instantiate(nodeTemplate) as GameObject;

        Node node = nodeGO.GetComponent<Node>();
        node._nodeManager = nodeManager;

        return node;
    }


    public void Setup()
    {
        transform.position = GetPosition();
        gameObject.name = "Node" + Index;
        transform.SetParent(_nodeManager.transform);
        AnchorCurve();
    }

    private void AnchorCurve()
    {
        if (PreviousNode == null) return;

        BezierCurve nodeCurve = gameObject.GetComponent<BezierCurve>();
        if (nodeCurve == null) nodeCurve = gameObject.AddComponent<BezierCurve>();
        nodeCurve.drawColor = Color.cyan;
        BezierPoint point1 = nodeCurve.AddPointAt(PreviousNode.transform.position);
        BezierPoint point2 = nodeCurve.AddPointAt(transform.position);

        point1.transform.parent = PreviousNode.transform;
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = _nodeManager.transform.position;
        return pos;
    }

}
