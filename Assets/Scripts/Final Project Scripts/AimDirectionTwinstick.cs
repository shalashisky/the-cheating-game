using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class AimDirectionTwinstick : MonoBehaviour
{
    public float joypadInputYR;
    public float joypadInputXR;
    public float joypadInputXL;
    public float joypadInputYL;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        
        //joypadInputXR = Mathf.RoundToInt(Input.GetAxis("MoveHorizontalX"));
        //joypadInputYR = Mathf.RoundToInt(Input.GetAxis("MoveVerticalX"));
        //joypadInputXL = (Input.GetAxis("MoveHorizontal"));
        //joypadInputYL = (Input.GetAxis("MoveVertical"));

        //if ((Mathf.Abs(joypadInputXR)>0) || (Mathf.Abs(joypadInputYR) > 0))
        //    transform.position = player.transform.position +new Vector3(joypadInputXR*3, 0f, -joypadInputYR*3);
        //else if ((Mathf.Abs(joypadInputXL) > 0) || (Mathf.Abs(joypadInputYL) > 0))
        //    transform.position = player.transform.position + new Vector3(joypadInputXL * 3, 0f, -joypadInputYL * 3);
        //else
        //    transform.position = player.transform.position + new Vector3(player.GetComponent<PlayerControllerTwinstick>().lastMovement.x * 3, 0f, player.GetComponent<PlayerControllerTwinstick>().lastMovement.z * 3);
    }
}
