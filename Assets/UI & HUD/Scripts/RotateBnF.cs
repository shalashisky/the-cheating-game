using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBnF : MonoBehaviour
{
    public float speed = 1f;
    public float maxRotation = 45f;

    void Update()
    {
        transform.rotation = Quaternion.Euler(maxRotation * Mathf.Sin(Time.time * speed), 0f, 0f);
    }
}
