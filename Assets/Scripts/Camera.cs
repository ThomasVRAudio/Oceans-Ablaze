using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 lookAtOffset;
    [SerializeField] private GameObject target;
    [SerializeField] private float rotSpeed;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotSpeed, Vector3.up);
            offset = camTurnAngle * offset;

        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, 0.1f);
        transform.LookAt(target.transform.position + lookAtOffset);
    }

}
