using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipConfigurations : MonoBehaviour
{
    [Header("Sloop Settings")]
    [SerializeField] private float sloop_maxSpeed = 20f;
    [SerializeField] private float sloop_rotSpeed = 20f;
    [SerializeField] private float sloop_acceleration = 2f;
    [SerializeField] private float sloop_deacceleration = 1f;

    [Header("Brigantine Settings")]
    [SerializeField] private float brig_maxSpeed = 20f;
    [SerializeField] private float brig_rotSpeed = 20f;
    [SerializeField] private float brig_acceleration = 2f;
    [SerializeField] private float brig_deacceleration = 1f;

    [Header("Galleon Settings")]
    [SerializeField] private float galleon_maxSpeed = 20f;
    [SerializeField] private float galleon_rotSpeed = 20f;
    [SerializeField] private float galleon_acceleration = 2f;
    [SerializeField] private float galleon_deacceleration = 1f;

    [Header("Cannon Settings")]
    [SerializeField] public float CannonForce = 800f;
    [SerializeField] public float MaxCannonDegrees = 30;
    [SerializeField] public GameObject CannonArc;
    [SerializeField] public Transform CannonArcParent;
    [SerializeField] public GameObject CannonSmokeVFX;

    public enum ShipType { Sloop, Brigantine, Galleon };

    public static ShipConfigurations Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public struct ShipData
    {
        public float maxSpeed;
        public float rotSpeed;
        public float acceleration;
        public float deacceleration;

        public ShipData(float maxSpeed, float rotSpeed, float acceleration, float deacceleration)
        {
            this.maxSpeed = maxSpeed;
            this.rotSpeed = rotSpeed;
            this.acceleration = acceleration;
            this.deacceleration = deacceleration;
        }
    }

    public ShipData GetData(ShipType type)
    {
        return type switch
        {
            ShipType.Sloop => new ShipData(sloop_maxSpeed, sloop_rotSpeed, sloop_acceleration, sloop_deacceleration),
            ShipType.Brigantine => new ShipData(brig_maxSpeed, brig_rotSpeed, brig_acceleration, brig_deacceleration),
            ShipType.Galleon => new ShipData(galleon_maxSpeed, galleon_rotSpeed, galleon_acceleration, galleon_deacceleration),
            _ => new ShipData(sloop_maxSpeed, sloop_rotSpeed, sloop_acceleration, sloop_deacceleration),
        };
    }


}
