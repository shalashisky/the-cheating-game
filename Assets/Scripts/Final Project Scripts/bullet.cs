using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class bullet : MonoBehaviour
{
    private Vector3 direction;
    public void Setup(Vector3 shootDir)
    {
        
        direction = shootDir;
        Debug.Log(direction);
    }

    public void Update()
    {
        transform.localEulerAngles = direction;
        transform.Translate(Vector3.forward);
    }
}
