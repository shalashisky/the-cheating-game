using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shotgun : MonoBehaviour
{
    public int pelletCount;
    public float spreadAngle;
    public GameObject bullet;
    public Transform BulletExit;
    List<Quaternion> pellets;
    // Start is called before the first frame update
    void Start()
    {
        pellets = new List<Quaternion>(pelletCount);
        for (int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
