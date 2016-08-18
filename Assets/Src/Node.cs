using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;
    private int _index;

    List<Vector2> points;
    public Node NextNode { get { return _nodeManager.GetNodeAtIndex(_index + 1); } }


    int step = 50;
    float distance;
    Vector2 direction;


    public void GeneratePoints()
    {
        Node nextNode = NextNode;
        if (nextNode != null)
        {
            direction = (Vector2)(nextNode.transform.position - transform.position).normalized;

            Image image = GetComponent<Image>();

            //Vector2 sizeDelta = image.rectTransform.rect.center;

            Vector2 startPos = transform.position;
            Vector2 endPos = nextNode.transform.position;

            //Debug.DrawLine(startPos, endPos, Color.red);

            distance = Vector2.Distance(startPos, endPos);

            points = new List<Vector2>();
            for (int i = step; i < distance; i += step)
            {
                Vector2 calculatedPoint = new Vector2(
                    transform.position.x + i * direction.x,
                    transform.position.y + i * direction.y
                );
                points.Add(calculatedPoint);
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        foreach (Vector2 point in points)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point, 10);
        }
    }

    public static Node Create(GameObject nodeTemplate, NodeManager nodeManager)
    {
        GameObject nodeGO = Instantiate(nodeTemplate) as GameObject;

        Node node = nodeGO.GetComponent<Node>();
        node._nodeManager = nodeManager;
        node._index = nodeManager.Nodes.Count;

        node.Setup();

        return node;
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
