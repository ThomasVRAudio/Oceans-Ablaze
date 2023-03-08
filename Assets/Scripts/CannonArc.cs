using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using TMathFunctions;

public class CannonArc : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] _vertices;
    private int[] _triangles;

    private LineRenderer _lineRenderer;

    public GameObject sphere;

    [SerializeField] private Vector3 arcWidth = new Vector3(0,0,-10);
    [SerializeField] private Vector2 arcUVMap = new Vector3(10,10);

    [Range(0.1f,10)]
    [SerializeField] private float arcLength = 5;
    public Vector3 rotationTest;

    [SerializeField] private GameObject ship;

    private bool isShowing = false;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

    }

    public void RenderArcMesh(Vector3 directionVector, Vector3 launchPosition, float force, Transform obj)
    {

       List<Vector3> line = GetArcPoints(directionVector, launchPosition, force, obj);

        _vertices = new Vector3[line.Count];

        for (int i = 0; i < line.Count; i ++)
            _vertices[i] = line[i];

        int height = line.Count / 2;
        _triangles = new int[height * 6];

        for (int i = 0, j = 0, t = 0; i < height - 1; i ++)
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

        Vector2[] uvs = new Vector2[_vertices.Length];

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                uvs[i] = new Vector2(x, y / (float)height);
                i++;
            }
        }

        mesh.Clear();

        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.uv = uvs;

        //StartCoroutine(ShowSphere(line));
        
    }

    //private IEnumerator ShowSphere(List<Vector3> points)
    //{
    //    if (isShowing) yield break;
    //    isShowing = true;

    //    float t = 0;

    //    while (t < 5f)
    //    {
    //        sphere.transform.position = points[(int)Mathfs.Remap(0f,5f,0f,points.Count-1f, t)];
    //        t += Time.deltaTime;
    //        yield return null;
    //    }

    //    isShowing = false;
    //}


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
