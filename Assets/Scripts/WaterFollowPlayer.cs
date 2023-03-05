using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        y  = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (target.position.x, y, target.position.z);
    }
}
