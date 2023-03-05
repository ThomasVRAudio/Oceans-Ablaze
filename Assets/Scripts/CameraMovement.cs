using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private float up = 0f;

    float xRot = 0, yRot = 0;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.E))
        {
            up = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            up = -1;
        }
        else
        {
            up = 0;
        }

        transform.position += (transform.rotation * new Vector3(x, up, y).normalized) * speed * Time.deltaTime;


        if (Input.GetMouseButton(1))
        {
            xRot = Input.mousePosition.x;
            yRot = Input.mousePosition.y;

        }

        transform.rotation = Quaternion.Euler(-yRot, xRot, 0);
    }
}
