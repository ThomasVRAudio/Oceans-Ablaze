using UnityEngine;

[RequireComponent(typeof(Cannons))]
public class Ship : MonoBehaviour
{
    [Header("Ship Type")]
    [SerializeField] private ShipConfigurations.ShipType shipType;

    [Header("Sails")]
    [SerializeField] private GameObject[] sails;
    [SerializeField] private Vector3 sailsUpPosition;

    private Vector3 _sailsDown;
    private float _sailsSpeed = 0f;
    private float _speed;
    private float _mousePos;

    private Cannons _cannons;
    private Cannons.CannonSide _side;

    private ShipConfigurations.ShipData _shipData;

    private void Start()
    {
        _speed = 0f;
        _sailsDown = sails[0].transform.localScale;

        _cannons = GetComponent<Cannons>();
        _shipData = ShipConfigurations.Instance.GetData(shipType);
    }

    public void SetCannonDegrees(Cannons.CannonSide side)
    {
        _side = side;

        _mousePos = Mathf.Lerp(_mousePos, Input.mousePosition.y, 0.5f * Time.deltaTime) ;
        float degrees = ShipConfigurations.Instance.MaxCannonDegrees * (_mousePos / Screen.height);

        _cannons.SetCannonRotations(_side, degrees);
    }

    public void StopAim() => _cannons.StopRenderArc();
    public void FireCannons() => _cannons.LaunchCannons(_side);
    

    public void Movement(float horMovement, float vertMovement)
    {

        transform.Rotate(0, horMovement * Time.deltaTime * _shipData.rotSpeed, 0);

        _speed += vertMovement > 0f ? _shipData.acceleration * Time.deltaTime : -_shipData.deacceleration * Time.deltaTime;

        if (_speed > _shipData.maxSpeed) _speed = _shipData.maxSpeed;
        if (_speed < 0f) _speed = 0f;

        transform.position += _speed * Time.deltaTime * transform.forward;

        SetFlags();
    }

    private void SetFlags()
    {
        if (_sailsSpeed > 1f) _sailsSpeed = 1f;
        if (_sailsSpeed < 0f) _sailsSpeed = 0f;

        if (_speed > 2)
        {
            foreach (var flag in sails)
            {
                flag.transform.localScale = Vector3.Lerp(sailsUpPosition, _sailsDown, _sailsSpeed);
            }
            _sailsSpeed += Time.deltaTime;
        }

        if (_speed < 1f)
        {
            foreach (var flag in sails)
            {
                flag.transform.localScale = Vector3.Lerp(_sailsDown, sailsUpPosition, 1 - _sailsSpeed);

            }
            _sailsSpeed -= Time.deltaTime;
        }
    }
}
