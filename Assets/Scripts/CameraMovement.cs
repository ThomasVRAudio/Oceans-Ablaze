using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private float up = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.E))
        {
            up = 1;
        } else if (Input.GetKey(KeyCode.Q)) {
            up = -1;
        } else
        {
            up = 0;
        }

        transform.position += new Vector3(x, up, y).normalized * speed * Time.deltaTime;
        
    }
}
