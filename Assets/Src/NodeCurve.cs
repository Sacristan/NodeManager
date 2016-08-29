using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeCurve : MonoBehaviour
{
    private Node _node;
    private Node _prevNode;

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

    private Node PrevNode
    {
        get
        {
            if (_prevNode == null) _prevNode = Node.PreviousNode;
            return _prevNode;
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
        InitateAnchorsIfRequired();
        CheckIfCurvePointsNeedToBeGenerated();
    }

    #endregion

    #region PublicMethods
    public NodeCurvePoint AddPointAt(Vector2 pos)
    {
        Debug.Log("Addding Point At: "+pos);
        NodeCurvePoint point = NodeCurvePoint.Create();
        point.transform.position = pos;
        AddPoint(point);

        return point;
    }

    public void AddPoint(NodeCurvePoint point, bool isAnchor=false)
    {
        point.IsAnchor = isAnchor;
        point.Curve = this;
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
        if (_creatingAnchors || points.Count > 0 || Node.IsDirty || PrevNode == null) return;
        _creatingAnchors = true;
        //Debug.Log(string.Format("I should create anchor points... Prev: {0} Curr: {1}", PrevNode, Node));

        _startAnchor = NodeCurvePoint.Create("_startAnchor");
        _endAnchor = NodeCurvePoint.Create("_endAnchor");

        _startAnchor.NextPoint = _endAnchor;

        AddPoint(_startAnchor, true);
        AddPoint(_endAnchor, true);

        NodeCurve prevCurve = PrevNode.GetComponentInChildren<NodeCurve>();

        _startAnchor.Curve = prevCurve;
        _startAnchor.transform.parent = prevCurve.transform;

        _startAnchor.transform.localPosition = Vector3.zero;
        _endAnchor.transform.localPosition = Vector3.zero;

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
