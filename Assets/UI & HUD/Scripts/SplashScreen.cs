using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{

    public GameObject soundManagerGO;
    public SoundManager soundManager;
    public float hideTime = 4f;


    // Start is called before the first frame update
    void Start()
    {
        soundManager = soundManagerGO.GetComponent<SoundManager>();
        Invoke("HideSplash", hideTime);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Submit") == 1)
        {
            HideSplash();
        }

        //add some animation

    }


    public void HideSplash()
    {
        this.gameObject.SetActive(false);
        soundManager.PlaySound(2);
        //play sound
    }
}
