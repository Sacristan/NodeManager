using UnityEngine;
using System.Collections;

public class NodeCurvePoint : MonoBehaviour
{
    private bool isAnchor;

    private NodeCurve _curve;
    private NodeCurvePoint _nextPoint;
    private LineRenderer _lineRenderer;

    private Vector3 _lastPointPosition;
    private Vector3 _lastNextPointPosition;

    private float _length;

    private const float MIN_DISTANCE_PER_POINT = 10f;
    private const float MAX_DISTANCE_PER_POINT = 50f;

    public NodeCurvePoint NextPoint
    {
        get { return _nextPoint; }
        set { _nextPoint = value; }
    }

    public float Length
    {
        get
        {
            if (IsDirty)
            {
                if (NextPoint == null)
                    _length = 0f;
                else
                    _length = Vector3.Distance(transform.position, NextPoint.transform.position);
            }
            return _length;
        }
    }

    public bool IsAnchor
    {
        get { return isAnchor; }
        set { isAnchor = value; }
    }


    public NodeCurve Curve
    {
        set { _curve = value; }
    }

    private LineRenderer LineRenderer
    {
        get
        {
            if (_lineRenderer == null)
            {
                _lineRenderer = gameObject.AddComponent<LineRenderer>();
                //_lineRenderer.SetWidth(1f, 1f);
                _lineRenderer.SetColors(Color.yellow, Color.yellow);
                Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
                _lineRenderer.material = whiteDiffuseMat;
            }
            return _lineRenderer;
        }
    }

    private Vector2 DirectionTowardsNextPoint
    {
        get
        {
            Vector2 result = Vector2.zero;
            if (_nextPoint != null) result = (_nextPoint.transform.position - transform.position).normalized;
            return result;
        }
    }

    private bool IsDirty
    {
        get { return (NextPoint == null) || (_lastPointPosition != transform.position || _lastNextPointPosition != NextPoint.transform.position); }
    }

    void Update()
    {
        ScaleLineRenderer();
    }

    void OnDestroy()
    {
        //Cleanup line renderer material from memory to avoid memory leaks
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) Destroy(renderer.material);
    }

    public void HandleCurveChange()
    {
        Debug.Log(string.Format("Called HandleCurveChange for {0} / Length: {1} ", gameObject.name, Length));

        if (Length > MAX_DISTANCE_PER_POINT) AddNewPointToCurve();
        else if (Length < MIN_DISTANCE_PER_POINT) RemoveMeFromCurve();
    }

    public static NodeCurvePoint Create(string pName=null)
    {
        string name = pName ?? "point";
        GameObject createdObject = Instantiate(NodeManager.KnobTemplate) as GameObject;
        createdObject.AddComponent<NodeCurvePoint>();
        return createdObject.GetComponent<NodeCurvePoint>();
    }

    private void ScaleLineRenderer()
    {
        if (NextPoint == null || !IsDirty) return;

        Vector3 pos1 = transform.position;
        Vector3 pos2 = NextPoint.transform.position;

        _lastPointPosition = pos1;
        _lastNextPointPosition = pos2;

        LineRenderer.SetPosition(0, (Vector2)pos1);
        LineRenderer.SetPosition(1, (Vector2)pos2);
    }

    private void AddNewPointToCurve()
    {
        Debug.Log("Called AddNewPointToCurve... " + DirectionTowardsNextPoint);

        if (DirectionTowardsNextPoint == Vector2.zero)
        {
            Debug.Log("DirectionTowardsNextPoint is zero... Skipping...");
            return;
        }

        Vector2 calculatedPosition = (Vector2)transform.position + (DirectionTowardsNextPoint * MAX_DISTANCE_PER_POINT);

        NodeCurvePoint newPoint = _curve.AddPointAt(calculatedPosition);

        newPoint.NextPoint = _nextPoint;
        this.NextPoint = newPoint;
    }

    private void RemoveMeFromCurve()
    {
        if (IsAnchor) return;
        Debug.Log("Called RemoveMeFromCurve...");
        
        //Handle transfer next point
        //_curve.RemovePoint(this);
        //Destroy(gameObject);
    }

}
