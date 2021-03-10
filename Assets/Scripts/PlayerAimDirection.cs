using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerAimDirection : MonoBehaviour
{
    public float joypadInputY2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        joypadInputY2 = Input.GetAxis("MoveVerticalX");
        transform.localEulerAngles = new Vector3(joypadInputY2 * 45, 0, 0);
    }
}
