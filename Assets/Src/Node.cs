using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;

    private Node _nextNode = null;
    private Node _prevNode = null;

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
           return _nodeManager.IndexForNode(this);
        }
    }

    private BezierCurve ThisCurve
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
        if (ThisCurve != null)
        {
            ThisCurve.ClearPoints();
            DestroyImmediate(ThisCurve);
        }

        if (NextCurve != null)
        {
            if (PreviousNode == null)
            {
                NextCurve.ClearPoints();
                DestroyImmediate(NextCurve);
            }
            else
            {
                BezierPoint bezierPoint = NextCurve.FirstPoint();
                Debug.Log(bezierPoint);
                bezierPoint.gameObject.transform.parent = PreviousNode.transform;
            }
        }
    }

    private void AnchorCurve()
    {
        if (PreviousNode == null) return;

        ThisCurve.drawColor = Color.cyan;
        BezierPoint point1 = ThisCurve.AddPointAt(PreviousNode.transform.position);
        BezierPoint point2 = ThisCurve.AddPointAt(transform.position);

        point1.transform.parent = PreviousNode.transform;
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = _nodeManager.transform.position;
        return pos;
    }

}
