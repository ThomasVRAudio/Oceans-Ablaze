using UnityEngine;
using TMathFunctions;
using static UnityEngine.GridBrushBase;

public class GalleonRoamingState : MonoBehaviour, IVesselState
{
    private GalleonStateMachine _stateMachine;

    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask rayLayerMask;
    [SerializeField] private float checkDegrees = 30;
    [SerializeField] private float raySideOffset = 2;

    private float _rotationDir = 0;
    public void OnStart(GalleonStateMachine SM)
    {
        _stateMachine = SM;
        _stateMachine.SetNextLocation();
        SetSails(false);
    }

    public void OnUpdate()
    {
        Steer();
        _stateMachine.Vessel.SetRotation(_rotationDir);

        if (VMathfs.InRange(_stateMachine.Vessel.transform.position, _stateMachine.TargetLocation, 10f)){
            _stateMachine.SetNextLocation();
        }
    }

    private void Steer()
    {
        Vector3 rayPosL = transform.position + transform.rotation * new Vector3(-raySideOffset, -2, 10);
        Vector3 rayPosR = transform.position + transform.rotation * new Vector3(raySideOffset, -2, 10);
        bool leftObstacle = Physics.Raycast(rayPosL + transform.forward, Quaternion.Euler(0, -checkDegrees, 0) * transform.forward, rayDistance, rayLayerMask);
        bool rightObstacle = Physics.Raycast(rayPosR + transform.forward, Quaternion.Euler(0, checkDegrees, 0) * transform.forward, rayDistance, rayLayerMask);


        if (leftObstacle)
        {
            _rotationDir = 1;
            return;
        }

        if (rightObstacle)
        {
            _rotationDir = -1;
            return;
        }

        float dotProduct = Vector3.Dot(_stateMachine.Vessel.transform.forward, 
            (_stateMachine.TargetLocation - _stateMachine.Vessel.transform.position).normalized);

        if (dotProduct < 0.9f)
        {
            Vector3 crossProduct = Vector3.Cross(_stateMachine.Vessel.transform.forward, 
                (_stateMachine.TargetLocation - _stateMachine.Vessel.transform.position).normalized);

            _rotationDir = crossProduct.y > 0 ? 1 : -1;
            
        }

        if (dotProduct > 0.99f)
        {
            _rotationDir = 0;
        }
    }

    private void SetSails(bool raise)
    {
        _stateMachine.Vessel.SetSails(raise);
    }

    private void OnDrawGizmos()
    {
        Vector3 rayPosL = transform.position + transform.rotation * new Vector3(-raySideOffset, - 2, 10);
        Vector3 rayPosR = transform.position + transform.rotation * new Vector3(raySideOffset, -2, 10);

        DrawLine(rayPosL, Quaternion.Euler(0, -checkDegrees, 0) * transform.forward, - raySideOffset);
        DrawLine(rayPosR, Quaternion.Euler(0, checkDegrees, 0) * transform.forward, raySideOffset);
    }

    private void DrawLine(Vector3 pos, Vector3 dir, float offset)
    {
        Gizmos.color = Physics.Raycast(pos, dir.normalized, rayDistance, rayLayerMask) ? Color.red : Color.green;
        Gizmos.DrawRay(pos, dir * rayDistance);      
    }
}
