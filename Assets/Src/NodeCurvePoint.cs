using UnityEngine;
using System.Collections;

public class NodeCurvePoint : MonoBehaviour
{
    private NodeCurvePoint _nextPoint;
    private LineRenderer _lineRenderer;

    private Vector3 _lastPointPosition;
    private Vector3 _lastNextPointPosition;

    private float _length;

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

    private LineRenderer LineRenderer
    {
        get
        {
            if (_lineRenderer == null)
            {
                _lineRenderer = gameObject.AddComponent<LineRenderer>();
                _lineRenderer.SetWidth(0.04f, 0.04f);
                _lineRenderer.SetColors(Color.white, Color.white);
                Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
                _lineRenderer.material = whiteDiffuseMat;
            }
            return _lineRenderer;
        }
    }

    private bool IsDirty
    {
        get { return _lastPointPosition != transform.position || _lastNextPointPosition != NextPoint.transform.position; }
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
