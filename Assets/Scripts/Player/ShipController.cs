using System;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class ShipController : MonoBehaviour
{
    private Action _cannonState;
    private Cannons.CannonSide _side;
    private Ship _ship;

    private void Start()
    {
        _ship = GetComponent<Ship>();
        _cannonState = AwaitCannonInput;
    }

    private void Update()
    {
        _cannonState();      
    }

    private void FixedUpdate()
    {
        _ship.Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void AwaitCannonInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _side = Cannons.CannonSide.Left;
            _cannonState = SetCannonDegrees;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _side = Cannons.CannonSide.Right;
            _cannonState = SetCannonDegrees;
        }
    }

    private void SetCannonDegrees()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ship.FireCannons();
            _cannonState = AwaitCannonInput;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _cannonState = AwaitCannonInput;
            _ship.StopAim();
        }

        _ship.SetCannonDegrees(_side);
    }


    
}
