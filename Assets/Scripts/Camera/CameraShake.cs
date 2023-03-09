using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float reductionSpeed = 2;
    [SerializeField] private float shakeSpeed = 10;
    [SerializeField] private float power = 3;

    [Header("Max Rotations")]
    [SerializeField] private float maxYawDeg = 50;
    [SerializeField] private float maxPitchDeg = 50;
    [SerializeField] private float maxRollDeg = 50;

    private float _trauma;
    private Quaternion _camStartRotation;  
    private int _seed = 124890;

    public static CameraShake Instance;
    
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _camStartRotation = cam.transform.localRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AddTrauma(0.2f);

        if (_trauma == 0) return;

        if (_trauma > 0)
            _trauma -= Time.deltaTime / reductionSpeed;

        if (_trauma > 1) _trauma = 1;
        if (_trauma < 0) _trauma = 0;

        Shake();
    }

    public void AddTrauma(float trauma)
    {
        _trauma += trauma;
    }

    private void Shake()
    {
        float rollShake = Mathf.Pow(maxRollDeg * _trauma * (Mathf.PerlinNoise(_seed, _seed + Time.time * shakeSpeed) * 2 - 1), power);
        float pitchShake = Mathf.Pow(maxRollDeg * _trauma * (Mathf.PerlinNoise(_seed + 800, _seed + 500 + Time.time * shakeSpeed) * 2 - 1), power);
        float yawShake = Mathf.Pow(maxRollDeg * _trauma * (Mathf.PerlinNoise(_seed + 200, _seed + 300 + Time.time * shakeSpeed) * 2 - 1), power);
        cam.transform.localRotation = Quaternion.Euler(yawShake, pitchShake, rollShake) * _camStartRotation;
    }
}
