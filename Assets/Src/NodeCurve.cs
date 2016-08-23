using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NodeCurve : MonoBehaviour
{
    private Node _node;

    public float Length { get { return 0f; } }

    void Awake()
    {
        _node = GetComponentInParent<Node>();

        NodeCurvePoint p1 = new GameObject("p1", typeof(NodeCurvePoint)).GetComponent<NodeCurvePoint>();
        NodeCurvePoint p2 = new GameObject("p2", typeof(NodeCurvePoint)).GetComponent<NodeCurvePoint>();
        NodeCurvePoint p3 = new GameObject("p3", typeof(NodeCurvePoint)).GetComponent<NodeCurvePoint>();
        NodeCurvePoint p4 = new GameObject("p4", typeof(NodeCurvePoint)).GetComponent<NodeCurvePoint>();
        NodeCurvePoint p5 = new GameObject("p5", typeof(NodeCurvePoint)).GetComponent<NodeCurvePoint>();

        p1.transform.parent = transform;
        p2.transform.parent = transform;
        p3.transform.parent = transform;
        p4.transform.parent = transform;
        p5.transform.parent = transform;

        p1.transform.localPosition = Vector3.zero;
        p2.transform.localPosition = Vector3.right;
        p3.transform.localPosition = new Vector3(2, 2, 0);
        p4.transform.localPosition = new Vector3(2, 5, 0);
        p5.transform.localPosition = new Vector3(3, 0, 0);

        p1.NextPoint = p2;
        p2.NextPoint = p3;
        p3.NextPoint = p4;
        p4.NextPoint = p5;
    }

    void Update()
    {

    }

}
