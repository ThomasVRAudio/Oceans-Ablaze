using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CannonArc : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] _vertices;
    private int[] _triangles;

    private LineRenderer _lineRenderer;

    public GameObject sphere;

    [SerializeField] private Vector3 arcWidth = new Vector3(0,0,-10);

    [Range(0.1f,10)]
    [SerializeField] private float arcLength = 5;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

    }

    public void RenderArcMesh(Vector3 directionVector, Vector3 launchPosition, float force, Transform obj)
    {
       // GetArcPoints(directionVector, launchPosition, force, obj);
        //return;

       List<Vector3> line = GetArcPoints(directionVector, launchPosition, force, obj);

        _vertices = new Vector3[line.Count];

        sphere.transform.position = line[0]; // DEBUG

        for (int i = 0; i < line.Count; i ++)
            _vertices[i] = line[i];

        int width = line.Count / 2;
        _triangles = new int[width * 6];

        for (int i = 0, j = 0, t = 0; i < width - 1; i ++)
        {
            _triangles[t] = 0 + j;
            _triangles[t + 1] = 1 + j;
            _triangles[t + 2] = 2 + j;

            _triangles[t + 3] = 1 + j;
            _triangles[t + 4] = 3 + j;
            _triangles[t + 5] = 2 + j;

            t += 6;
            j += 2;
        }

        mesh.Clear();

        mesh.vertices = _vertices;
        mesh.triangles = _triangles;

    }


    public List<Vector3> GetArcPoints(Vector3 directionVector, Vector3 launchPosition, float force, Transform obj)
    {
        launchPosition += directionVector;
        float mass = 1f;
        float gravity = Physics.gravity.y;
        List<Vector3> linePoints = new();

        float timeStepInterval = 0.1f;
        int maxSteps = (int)(arcLength / timeStepInterval);
        

        float velocity = force / mass * Time.fixedDeltaTime;

        for (int i = 0; i < maxSteps; i++)
        {
            Vector3 calculatedPosition = launchPosition + directionVector.normalized * velocity * i * timeStepInterval;
            calculatedPosition.y += gravity / 2 * Mathf.Pow(i * timeStepInterval, 2);

            linePoints.Add(calculatedPosition);
            linePoints.Add(calculatedPosition + obj.transform.rotation * arcWidth); // kill for lineRenderer
        }

        //_lineRenderer.positionCount = linePoints.Count;
        //for (int i = 0; i < maxSteps; i++)
        //{
        //    _lineRenderer.SetPosition(i, linePoints[i] + obj.transform.rotation * offsetDebug);
        //}

        return linePoints;
    }
}
