using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMathFunctions;
using System.Threading.Tasks;

public class GalleonRoamingState : MonoBehaviour, IVesselState
{
    private GalleonStateMachine _stateMachine;
    private Vector3 _targetLocation;

    public float DotProductDebug;
    public void OnStart(GalleonStateMachine SM)
    {
        _stateMachine = SM;
        _stateMachine.SetNextLocation();
        SetSails(false);
    }

    public void OnUpdate()
    {
        Steer();

        if (VMathfs.InRange(_stateMachine.Vessel.transform.position, _stateMachine.TargetLocation, 10f)){
            _stateMachine.SetNextLocation();
        }
    }

    private void Steer()
    {
        float dotProduct = Vector3.Dot(_stateMachine.Vessel.transform.forward, (_stateMachine.TargetLocation - _stateMachine.Vessel.transform.position).normalized);
        if (dotProduct < 0.998f)
        {
            _stateMachine.Vessel.Movement(1);
        }
        else
        {
            _stateMachine.Vessel.Movement(0);
        }
    }

    private void SetSails(bool raise)
    {
        _stateMachine.Vessel.SetSails(raise);
    }
}
