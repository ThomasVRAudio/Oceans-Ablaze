using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cannons))]
public class ShipController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float rotSpeed = 20f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deacceleration = 1f;

    [SerializeField] private GameObject[] flags;
    [SerializeField] private Vector3 flagsUp;

    private Vector3 _flagsDown;
    private float _flagSpeed = 0f;

    private float _speed;

    private Cannons _cannons;
    private Cannons.CannonSide _side;

    private Action CannonState;

    private float _mousePos;

    private void Start()
    {
        _speed = 0f;
        _flagsDown = flags[0].transform.localScale;

        _cannons = GetComponent<Cannons>();

        CannonState = AwaitCannonInput;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        CannonState();       
    }

    private void AwaitCannonInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _side = Cannons.CannonSide.Left;
            CannonState = SetCannonDegrees;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _side = Cannons.CannonSide.Right;
            CannonState = SetCannonDegrees;
        }

    }

    private void SetCannonDegrees()
    {
        if (Input.GetMouseButtonDown(0))
            _cannons.LaunchCannons(_side);

        if (Input.GetKeyDown(KeyCode.Escape))
            CannonState = AwaitCannonInput;

        _mousePos = Mathf.Lerp(_mousePos, Input.mousePosition.y, 0.5f * Time.deltaTime) ;
        float degrees = Cannons.MAX_CANNON_DEGREES * (_mousePos / Screen.height);

        _cannons.SetCannonRotations(_side, degrees);
    }

    private void Movement()
    {
        float horMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        transform.Rotate(0, horMovement * Time.deltaTime * rotSpeed, 0);

        _speed += vertMovement > 0f ? acceleration * Time.deltaTime : -deacceleration * Time.deltaTime;

        if (_speed > maxSpeed) _speed = maxSpeed;
        if (_speed < 0f) _speed = 0f;

        if (_flagSpeed > 1f) _flagSpeed = 1f;
        if (_flagSpeed < 0f) _flagSpeed = 0f;

        if (_speed > 2)
        {
            foreach (var flag in flags)
            {
                flag.transform.localScale = Vector3.Lerp(flagsUp, _flagsDown, _flagSpeed);
            }
            _flagSpeed += Time.deltaTime;
        }

        if (_speed < 1f)
        {
            foreach (var flag in flags)
            {
                flag.transform.localScale = Vector3.Lerp(_flagsDown, flagsUp, 1 - _flagSpeed);

            }
            _flagSpeed -= Time.deltaTime;
        }

        transform.position += _speed * Time.deltaTime * transform.forward;
    }
}
