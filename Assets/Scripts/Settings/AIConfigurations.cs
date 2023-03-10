using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConfigurations : MonoBehaviour
{
    [Header("Max Location Space")]
    public float MapWidth;
    public float MapHeight;

    public static AIConfigurations Instance;

    public bool Debug;

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

    private void OnDrawGizmos()
    {
        if (!Debug) return;
        
        Gizmos.color= Color.red;
        Gizmos.DrawWireCube(new Vector3(0, 2, 0), new Vector3(MapWidth, 10, MapHeight));
    }
}
