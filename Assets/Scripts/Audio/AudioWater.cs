using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWater : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Play(AudioManager.Instance.SFX_Ambience, Vector3.zero);
        AudioManager.Instance.Play(AudioManager.Instance.SFX_WoodCreak, ship.transform.position, ship.transform);
    }

    
}
