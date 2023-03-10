using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GalleonRoamingState), typeof(Ship))]
public class GalleonStateMachine : MonoBehaviour
{
    [SerializeField] private LayerMask spawnLocationMask;
    private IVesselState _currentState;
    private List<Vector3> _travelLocations = new();

    private GalleonAttackingState AttackingState;
    private GalleonChasingState ChasingState;
    private GalleonRoamingState RoamingState;

    public GameObject Target { get; private set; }
    public Vector3 TargetLocation { get; private set; }
    public int CurrentLocationIndex { get; private set; }   
    public Ship Vessel { get; private set; }

    public bool debug;


    private void Awake()
    {
        AttackingState = GetComponent<GalleonAttackingState>();
        ChasingState = GetComponent<GalleonChasingState>();
        RoamingState = GetComponent<GalleonRoamingState>();
        Vessel = GetComponent<Ship>();


    }

    private void Start()
    {
        float mapWidth = AIConfigurations.Instance.MapWidth;
        float mapHeight = AIConfigurations.Instance.MapHeight;

        for (int i = 0; i < 10; i++)
        {
            Collider[] coll;
            Vector3 location;
            do
            {
                location = new Vector3(Random.Range(-mapWidth / 2, mapWidth / 2), 0, Random.Range(-mapHeight / 2, mapHeight / 2));

                coll = Physics.OverlapSphere(location, 20, spawnLocationMask);

            } while (coll.Length > 0);
            _travelLocations.Add(location);
        }

        SetState(RoamingState);
    }

    private void Update()
    {  
        _currentState.OnUpdate();
    }

    public void SetState(IVesselState state)
    {
        _currentState = state;
        _currentState.OnStart(this);
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    public void SetNextLocation()
    {
        int index; 

        do
        {
          index = Random.Range(0, _travelLocations.Count);
        } while (CurrentLocationIndex == index);

        CurrentLocationIndex = index;
        TargetLocation = _travelLocations[CurrentLocationIndex];
    }

    private void OnDrawGizmos()
    {
        if (_travelLocations == null || !debug) return;
        
        Gizmos.color = Color.yellow;

        foreach (var location in _travelLocations)
        {
            Gizmos.color = location == _travelLocations[CurrentLocationIndex] ? Color.yellow : Color.red;
            Gizmos.DrawWireSphere(location, 5);
        }
    }
}
