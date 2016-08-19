using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;

    private Node _nextNode = null;
    private Node _prevNode = null;

    private int _index;
    private BezierCurve _prevCurve;
    private BezierCurve _nextCurve;

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

    private BezierCurve PrevCurve
    {
        get
        {
            _prevCurve = gameObject.GetComponent<BezierCurve>();
            if (_prevCurve == null) _prevCurve = gameObject.AddComponent<BezierCurve>();
            return _prevCurve;
        }
    }

    private BezierCurve NextCurve
    {
        get
        {
            if (NextNode == null)
            {
                return null;
            }
            else
            {
                if (_nextCurve == null) _nextCurve = NextNode.gameObject.GetComponent<BezierCurve>();
                return _nextCurve;
            }

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

    public void Cleanup()
    {
        if (PrevCurve != null) PrevCurve.ClearPoints();
        if (NextCurve != null) NextCurve.ClearPoints();
        DestroyImmediate(gameObject);
    }

    private void AnchorCurve()
    {
        if (PreviousNode == null) return;

        PrevCurve.drawColor = Color.cyan;
        BezierPoint point1 = PrevCurve.AddPointAt(PreviousNode.transform.position);
        BezierPoint point2 = PrevCurve.AddPointAt(transform.position);

        point1.transform.parent = PreviousNode.transform;
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = _nodeManager.transform.position;
        return pos;
    }

}
