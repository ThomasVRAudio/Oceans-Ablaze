using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class Cannons : MonoBehaviour
{
    public enum CannonSide { Left, Right, Both };

    [SerializeField] private GameObject[] CannonsLeft;
    [SerializeField] private GameObject[] CannonsRight;

    [SerializeField] private float cannonForce = 50f;
    [SerializeField] private Vector3 cannonRotationOffset;

    private float _rotationInDeg = 90;
    public const float MAX_CANNON_DEGREES = 30;

    [Header("Debugging")]
    [SerializeField] private bool debug = false;

    [SerializeField] private CannonArc visualArc;

    private void Update()
    {
        DrawArc(); // TEST
    }
    public void SetCannonRotations(CannonSide side, float degrees)
    {
        if (degrees > MAX_CANNON_DEGREES) 
            degrees = MAX_CANNON_DEGREES;

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
            case CannonSide.Both:
                cannonArray = CannonsLeft.Concat(CannonsRight).ToArray();
                break;
            default:
                break;
        }

        for (int i = 0; i < cannonArray.Length; i++)
        {
            cannonArray[i].transform.localRotation = Quaternion.AngleAxis(90 - degrees, Vector3.forward);

        }
        
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
            case CannonSide.Both:
                cannonArray = CannonsLeft.Concat(CannonsRight).ToArray();
                break;
            default:
                break;
        }

        if (cannonArray == null) return;

        for (int i = 0; i < cannonArray.Length; i++)
        {
            GameObject cBall = CannonballPool.Instance.GetCannonball();
            if (cBall == null) return;

            cBall.transform.SetPositionAndRotation(cannonArray[i].transform.position, Quaternion.identity);
            Transform cannonPos = cannonArray[i].transform;

            Quaternion dir = cannonPos.rotation * Quaternion.Euler(_rotationInDeg, 90, 0);

            cBall.GetComponent<Rigidbody>().AddForce(
                 dir * Vector3.forward * cannonForce , 
                ForceMode.Force);

            CannonballPool.Instance.DestroyCannonball(cBall, 3f);
        }
    }

    private void DrawArc()
    {
        Transform cannonPos = CannonsLeft[0].transform;
        Quaternion dir = cannonPos.rotation * Quaternion.Euler(-90, 90, 0);
        visualArc.RenderArcMesh(dir * Vector3.forward, cannonPos.position, cannonForce, CannonsLeft[0].transform);
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;

        Transform cannonPos = CannonsLeft[0].transform;
        Quaternion dir =  cannonPos.rotation * Quaternion.Euler(-90,90,0);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cannonPos.position, dir * Vector3.forward * 10);
    }

}
