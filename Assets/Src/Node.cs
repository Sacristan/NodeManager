using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    private NodeManager _nodeManager;
    private int _index;

    public Node NextNode { get { return _nodeManager.GetNodeAtIndex(_index + 1); } }

    int step = 50;
    private List<GameObject> knobs;

    public void GenerateKnobs()
    {
        Node nextNode = NextNode;
        if (nextNode != null)
        {
            Vector2 direction = (nextNode.transform.position - transform.position).normalized;

            //Image image = GetComponent<Image>();
            //Vector2 sizeDelta = image.rectTransform.rect.center;

            Vector2 startPos = transform.position;
            Vector2 endPos = nextNode.transform.position;

            //Debug.DrawLine(startPos, endPos, Color.red);

            float distance = Vector2.Distance(startPos, endPos);

            CleanUpKnobs();
            for (int i = step; i < distance; i += step)
            {
                Vector2 calculatedPoint = new Vector2(
                    transform.position.x + i * direction.x,
                    transform.position.y + i * direction.y
                );
                CreateKnobAtPoint(calculatedPoint);
            }
        }
    }

    public void CleanUpKnobs()
    {
        foreach (GameObject knob in knobs)
        {
            DestroyImmediate(knob);
        }

        knobs = new List<GameObject>();
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

    private void CreateKnobAtPoint(Vector2 pos)
    {
        GameObject knob = Instantiate(_nodeManager.KnobTemplate, pos, Quaternion.identity) as GameObject;
        knobs.Add(knob);
        knob.transform.parent = transform;
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
