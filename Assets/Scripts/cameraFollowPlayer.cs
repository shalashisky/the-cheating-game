using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 7.27f, -14.35f);
    }
}
