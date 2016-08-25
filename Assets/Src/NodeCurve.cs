using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeCurve : MonoBehaviour
{
    private Node _node;
    private Node _nextNode;

    private NodeCurvePoint _startAnchor;
    private NodeCurvePoint _endAnchor;

    private List<NodeCurvePoint> points = new List<NodeCurvePoint>();
    private float _length;
    private float _lastLength;

    private bool _wasDirty = false;
    private int _lastFrameWhenWasDirty = 0;
    private float _lastGeneratedTime = 0f;

    private const float WAITING_TRESHOLD = 1f;

    private bool _creatingAnchors = false;

    #region PublicGetters
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
    #endregion

    #region PrivateGetters   
    private bool IsDirty
    {
        get
        {
            bool isDirty = !(_lastLength > 0) || Mathf.Abs(_lastLength - Length) > 0.1f;
            return isDirty;
        }
    }

    private Node Node
    {
        get
        {
            if (_node == null) _node = GetComponentInParent<Node>();
            return _node;
        }
    }

    private Node NextNode
    {
        get
        {
            if (_nextNode == null) _nextNode = Node.NextNode;
            return _node;
        }
    }
    #endregion

    #region UnityMethods
    void Awake()
    {
        GenerateCurvePoints();
    }

    void Update()
    {
        CheckIfCurvePointsNeedToBeGenerated();
    }
    #endregion

    #region PublicMethods
    public void AddPoint(NodeCurvePoint point, bool isAnchor)
    {
        point.IsAnchor = isAnchor;
        points.Add(point);

        point.transform.parent = transform;
    }

    public void RemovePoint(NodeCurvePoint point)
    {
        if (point.IsAnchor) Debug.LogError("NodeCurve Cannot Remove Anchor Points");
        points.Remove(point);
        Destroy(point.gameObject);
    }
    #endregion

    #region PrivateMethods
    private void CheckIfCurvePointsNeedToBeGenerated()
    {
        InitateAnchorsIfRequired();
        if (Time.realtimeSinceStartup - _lastGeneratedTime < WAITING_TRESHOLD) return;
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

        yield return new WaitForSeconds(WAITING_TRESHOLD);

        if (!IsDirty && entryDirtyFrame == _lastFrameWhenWasDirty)
        {
            //Debug.Log("Just stopped... Should generate!");
            GenerateCurvePoints();
        }
    }

    private void InitateAnchorsIfRequired()
    {
        if (points.Count > 0 || _creatingAnchors) return;
        _creatingAnchors = true;
        Debug.Log("I should create anchor points...");

        _startAnchor = new GameObject("_startAnchor", typeof(NodeCurvePoint)).GetComponent<NodeCurvePoint>();
        _endAnchor = new GameObject("_endAnchor", typeof(NodeCurvePoint)).GetComponent<NodeCurvePoint>();

        _startAnchor.NextPoint = _endAnchor;

        AddPoint(_startAnchor, true);
        AddPoint(_endAnchor, true);

        _creatingAnchors = false;
    }

    private void GenerateCurvePoints()
    {
        _lastGeneratedTime = Time.realtimeSinceStartup;

        Debug.Log("Regenerating points...");

        foreach (NodeCurvePoint point in points)
        {
            point.HandleCurveChange();
        }
    }
    #endregion

}
