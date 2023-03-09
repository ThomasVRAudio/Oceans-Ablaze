using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CannonballPool : MonoBehaviour
{
    public static CannonballPool Instance;

    [SerializeField] private GameObject CannonballPrefab;
    [SerializeField] private int cannonballCount;

    private readonly List<GameObject> _cannons = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        for (int i = 0; i < cannonballCount; i++)
        {
            GameObject obj = Instantiate(CannonballPrefab, transform);
            _cannons.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetCannonball()
    {
        for (int i = 0; i < cannonballCount; i++)
        {
            if (!_cannons[i].activeInHierarchy)
            {
                _cannons[i].SetActive(true);
                return _cannons[i];
            }
        }
        return null;
    }

    public async void DestroyCannonball(GameObject cannonball, float delay = 0)
    {
        delay *= 1000;
        await Task.Delay((int)delay);

        if (cannonball == null) return;

        cannonball.SetActive(false);
        cannonball.GetComponent<Rigidbody>().velocity= Vector3.zero;

    }

}
