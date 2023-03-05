using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SetLandColours : MonoBehaviour 
{
    [SerializeField] Color[] colours;

    private Material _material;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;

        

       
    }
}
