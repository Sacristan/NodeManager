using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;

    private Node _nextNode = null;
    private Node _prevNode = null;

    private bool _isDirty = true;

    public bool IsDirty
    {
        get { return _isDirty;  }
    }

    public Node NextNode
    {
        get
        {
            if (_nextNode == null) _nextNode = _nodeManager.GetNodeAtIndex(Index + 1);
            if (this.Equals(_nextNode)) Debug.Log("Next Node Cannot be Equal to Current One");
            return _nextNode;
        }
    }

    public Node PreviousNode
    {
        get
        {   if (Index < 0) return null;
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
        this._isDirty = false;
    }

    public void TransferAnchors()
    {
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = _nodeManager.transform.position;
        return pos;
    }

}
