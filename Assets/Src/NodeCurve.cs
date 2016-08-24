using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeCurve : MonoBehaviour
{
    private Node _node;
    private List<NodeCurvePoint> points = new List<NodeCurvePoint>();
    private float _length;
    private float _lastLength;

    private bool _wasDirty = false;
    private int _lastFrameWhenWasDirty = 0;

    #region PublicProperties
    public float Length
    {
        get
        {
            float result = 0f;
            foreach (NodeCurvePoint nodeCurvePoint in points)
                result += nodeCurvePoint.Length;

            _length = result;
            return _length;
        }
    }

    private bool IsDirty
    {
        get
        {
            bool isDirty = !(_lastLength > 0) || Mathf.Abs(_lastLength - Length) > 0.1f;
            return isDirty;
        }
    }
    #endregion


    #region UnityMethods
    void Awake()
    {
        _node = GetComponentInParent<Node>();
        GenerateCurvePoints();
    }

    void Update()
    {
        CheckIfCurvePointsNeedToBeGenerated();
    }
    #endregion

    #region PrivateMethods
    private void CheckIfCurvePointsNeedToBeGenerated()
    {
        bool isDirty = IsDirty;
        float length = Length;

        if (!isDirty)
        {
            if (_wasDirty && Time.frameCount >= _lastFrameWhenWasDirty + 1)
            {
                StartCoroutine(GenerateCurvePointsRoutine());
            }

            _wasDirty = false;
        }
        else
        {
            _wasDirty = true;
            _lastFrameWhenWasDirty = Time.frameCount;
        }

        _lastLength = length;
    }

    private IEnumerator GenerateCurvePointsRoutine()
    {
        int entryDirtyFrame = _lastFrameWhenWasDirty;

        yield return new WaitForSeconds(1f);

        if (!IsDirty && entryDirtyFrame == _lastFrameWhenWasDirty)
        {
            Debug.Log("Just stopped... Should generate!");
        }
    }

    private void GenerateCurvePoints()
    {
        Debug.Log("Cleaning up points");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Regenerating points...");

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
        p3.transform.localPosition = new Vector3(Random.Range(0, 5), 2, 0);
        p4.transform.localPosition = new Vector3(Random.Range(0, 5), 5, 0);
        p5.transform.localPosition = new Vector3(Random.Range(0, 5), 0, 0);

        p1.NextPoint = p2;
        p2.NextPoint = p3;
        p3.NextPoint = p4;
        p4.NextPoint = p5;

        points.Add(p1);
        points.Add(p2);
        points.Add(p3);
        points.Add(p4);
        points.Add(p5);
    }
    #endregion

}
