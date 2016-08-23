using UnityEngine;
using System.Collections;

public class NodeCurvePoint : MonoBehaviour
{
    private NodeCurvePoint _nextPoint;
    private LineRenderer _lineRenderer;

    private Vector3 _lastPointPosition;
    private Vector3 _lastNextPointPosition;

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

    public NodeCurvePoint NextPoint
    {
        get { return _nextPoint;  }
        set { _nextPoint = value; }
    }

    void Update()
    {
        ScaleLineRenderer();
    }

    void OnDestroy()
    {
        //Cleanup line renderer material from memory to avoid memory leaks
        Renderer renderer = GetComponent<Renderer>();
        if(renderer!=null) Destroy(renderer.material);
    }

    private void ScaleLineRenderer()
    {
        if (NextPoint == null) return;

        if (_lastPointPosition != transform.position || _lastNextPointPosition != NextPoint.transform.position)
        {
            _lastPointPosition = transform.position;
            _lastNextPointPosition = NextPoint.transform.position;

            LineRenderer.SetPosition(0, (Vector2)transform.position);
            LineRenderer.SetPosition(1, (Vector2)NextPoint.transform.position);
        }
    }
}
