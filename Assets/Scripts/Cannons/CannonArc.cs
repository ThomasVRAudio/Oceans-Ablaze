using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CannonArc : MonoBehaviour
{
    [Range(0.1f,10)]
    [SerializeField] private float arcLength = 5;
    [SerializeField] private Vector3 arcWidth = new(0,0,-10);
    [SerializeField] private LayerMask arcMask;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    public async void StopRenderArcMesh()
    {
        await Task.Delay(10);
        _mesh.Clear();
    }

    public void RenderArcMesh(Vector3 directionVector, float force, Transform cannonTransform, Cannons.CannonSide side)
    {
        Vector3 canPos;
        float widthDirection;
        if (side == Cannons.CannonSide.Right)
        {
            canPos = cannonTransform.position + cannonTransform.rotation * arcWidth;
            widthDirection = -1;
        }
        else
        {
            widthDirection = 1;
            canPos = cannonTransform.position;
        }

       List<Vector3> line = GetArcPoints(directionVector, force, canPos, cannonTransform.rotation, widthDirection);

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

        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = uvs;
        
    }


    public List<Vector3> GetArcPoints(Vector3 directionVector, float force, Vector3 cannonTransform, Quaternion cannonRotation, float widthDirection)
    {
        Vector3 launchPosition = cannonTransform + directionVector;
        float mass = 1f;
        float gravity = Physics.gravity.y;
        List<Vector3> linePoints = new();

        float timeStepInterval = 0.002f;
        int maxSteps = (int)(arcLength / timeStepInterval);
        

        float velocity = force / mass * Time.fixedDeltaTime;

        for (int i = 0; i < maxSteps; i++)
        {
            Vector3 calculatedPosition = launchPosition + directionVector.normalized * velocity * i * timeStepInterval;
            calculatedPosition.y += gravity / 2 * Mathf.Pow(i * timeStepInterval, 2);

            linePoints.Add(calculatedPosition);
            linePoints.Add(calculatedPosition + cannonRotation * (arcWidth * widthDirection) );

            Collider[] OS = Physics.OverlapSphere(calculatedPosition, 0.5f, arcMask);
            if ( OS.Length != 0)
                return linePoints;            

        }

        return linePoints;
    }
}
