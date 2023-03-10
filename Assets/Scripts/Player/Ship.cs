using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Cannons))]
public class Ship : MonoBehaviour
{
    [Header("Ship Type")]
    [SerializeField] private ShipConfigurations.ShipType shipType;

    [Header("Sails")]
    [SerializeField] private GameObject[] sails;
    [SerializeField] private Vector3 sailsUpPosition;

    private Vector3 _sailsDownPosition;

    private float _speed;
    private float _mousePos;
    private float _movement;

    private Cannons _cannons;
    private Cannons.CannonSide _side;

    private ShipConfigurations.ShipData _shipData;

    private bool _changingSails = false;
    private bool _sailsAreRaised;

    private void Awake()
    {
        _sailsDownPosition = sails[0].transform.localScale;
        _sailsAreRaised = true;

        foreach (var flag in sails)
        {
            flag.transform.localScale = sailsUpPosition;
        }
    }
    private void Start()
    {
        _speed = 0f;
        
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

    private void FixedUpdate()
    {
        Movement();
    }

    public void SetRotation(float horMovement)
    {
        transform.Rotate(0, horMovement * Time.deltaTime * _shipData.rotSpeed, 0);
    }

    private void Movement()
    {
        _speed += _movement > 0f ? _shipData.acceleration * Time.deltaTime : -_shipData.deacceleration * Time.deltaTime;

        if (_speed > _shipData.maxSpeed) _speed = _shipData.maxSpeed;
        if (_speed < 0f) _speed = 0f;

        transform.position += _speed * Time.deltaTime * transform.forward;
    }

    public async void SetSails(bool raise)
    {
        if (_changingSails)
            return;

        if (_sailsAreRaised == raise)
            return;

        _changingSails = true;
        _sailsAreRaised = raise;

        float t = 0;

        Vector3 a;
        Vector3 b;

        a = raise ? _sailsDownPosition : sailsUpPosition;
        b = raise ? sailsUpPosition : _sailsDownPosition;

        while (t < ShipConfigurations.Instance.FlagSpeed)
        {
            foreach (var flag in sails)
            {
                flag.transform.localScale = Vector3.Lerp(a, b, t / ShipConfigurations.Instance.FlagSpeed);
            }
            t += Time.deltaTime;
            await Task.Yield();
        }

        _changingSails = false;
        _movement = raise ? -1 : 1;
    }
}
