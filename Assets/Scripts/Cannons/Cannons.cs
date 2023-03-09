using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Cannons : MonoBehaviour
{
    public enum CannonSide { Left, Right };

    [SerializeField] private GameObject[] CannonsLeft;
    [SerializeField] private GameObject[] CannonsRight;    
    [SerializeField] private Vector3 cannonRotationOffset;

    private float _rotationInDeg = 90;
    private CannonArc _visualArc;

    private Vector3 _velocity;
    private Vector3 _previousVelocity;

    public void Start()
    {
        _visualArc = Instantiate(ShipConfigurations.Instance.CannonArc, ShipConfigurations.Instance.CannonArcParent).GetComponent<CannonArc>();
    }

    public void SetCannonRotations(CannonSide side, float degrees)
    {
        if (degrees > ShipConfigurations.Instance.MaxCannonDegrees) 
            degrees = ShipConfigurations.Instance.MaxCannonDegrees;

        if (degrees < 0)
            degrees = 0;

        if (side == CannonSide.Right)
        {
            degrees *= -1;
        }
        GameObject[] cannonArray = null;

        switch (side)
        {

            case CannonSide.Left:
                cannonArray = CannonsLeft;
                break;
            case CannonSide.Right:
                cannonArray = CannonsRight;
                break;
            default:
                break;
        }

        for (int i = 0; i < cannonArray.Length; i++)
        {
            cannonArray[i].transform.localRotation = Quaternion.AngleAxis(90 - degrees, Vector3.forward);

        }

        DrawArc(side);       
    }

    private void DrawArc(CannonSide side)
    {
        Transform cannon = side == CannonSide.Left ? CannonsLeft[0].transform : CannonsRight[0].transform;
        float rot = side == CannonSide.Left ? -90 : 90;

        Quaternion dir = cannon.rotation * Quaternion.Euler(rot, 90, 0);
        _visualArc.RenderArcMesh(dir * Vector3.forward, ShipConfigurations.Instance.CannonForce, cannon, side);
    }

    public void LaunchCannons(CannonSide side)
    {
        GameObject[] cannonArray = null;

        switch (side)
        {
            case CannonSide.Left:
                cannonArray = CannonsLeft;
                _rotationInDeg = -90;
                break;
            case CannonSide.Right:
                cannonArray = CannonsRight;
                _rotationInDeg = 90;
                break;
            default:
                break;
        }

        if (cannonArray == null) return;

        for (int i = 0; i < cannonArray.Length; i++)
        {
            FireCannonBall(i, cannonArray);
        }

        StopRenderArc();
    }

    public async void FireCannonBall(int index, GameObject[] cannonArray)
    {
        int delay = Random.Range(0, 1000);
        await Task.Delay(delay);

        GameObject cBall = CannonballPool.Instance.GetCannonball();
        if (cBall == null) return;

        cBall.transform.SetPositionAndRotation(cannonArray[index].transform.position, Quaternion.identity);
        Transform cannonPos = cannonArray[index].transform;

        Quaternion dir = cannonPos.rotation * Quaternion.Euler(_rotationInDeg, 90, 0);

        cBall.GetComponent<Rigidbody>().AddForce(
             dir * Vector3.forward * ShipConfigurations.Instance.CannonForce,
            ForceMode.Force);

       cBall.GetComponent<Rigidbody>().velocity += _velocity; 

        GameObject fireVFX = Instantiate(ShipConfigurations.Instance.CannonSmokeVFX, cannonArray[index].transform.position, dir, transform);
        Destroy(fireVFX, 2f);
        CannonballPool.Instance.DestroyCannonball(cBall, 3f);
    }

    public void StopRenderArc() => _visualArc.StopRenderArcMesh();

    private void FixedUpdate()
    {
        _velocity = (transform.position - _previousVelocity) / Time.deltaTime;
        _previousVelocity = transform.position;
    }
}
