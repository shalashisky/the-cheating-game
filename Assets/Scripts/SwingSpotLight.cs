using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSpotLight : MonoBehaviour
{
    private bool moveLeft = true;
    private float countdown;
    private float waitToSwitch;

    public float timer = 1000;
    public float waitTime = 500;
    public float direction = 0.1f;

    void Start()
    {
        countdown = timer;
        waitToSwitch = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(countdown+","+waitToSwitch);

        if (countdown<=0)
        {
            waitToSwitch-= Time.deltaTime;

            if (waitToSwitch<=0)
            {
                waitToSwitch = waitTime;
                countdown = timer*2;
                direction = -direction;
            }
            
        } else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
            transform.localEulerAngles.y + direction, transform.localEulerAngles.z);
            countdown -= Time.deltaTime;
        }


    }
}
