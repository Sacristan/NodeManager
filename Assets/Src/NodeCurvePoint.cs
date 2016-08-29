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
    private const float MAX_DISTANCE_PER_POINT = 25f;

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
        Debug.Log(string.Format("Called HandleCurveChange for {0} / {1}", gameObject.name, gameObject.GetHashCode()));
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
}
