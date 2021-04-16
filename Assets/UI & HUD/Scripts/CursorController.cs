using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    private PlayerInput inputManager;

    public float joypadInputXL;
    public float joypadInputYL;

    private Vector3 cursorVelocity;

    public float cursorSpeed = 4f;
    float moveHorizontalP1;
    bool playerClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        inputManager = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {

        joypadInputXL = Input.GetAxis(inputManager.controller.lAnalogX);
        joypadInputYL = Input.GetAxis(inputManager.controller.lAnalogY);

        cursorVelocity = new Vector3(joypadInputXL, joypadInputYL, 0f);

        transform.Translate(cursorVelocity * cursorSpeed * Time.deltaTime);

        if (playerClicked)
        {
            Invoke("clickTimer", .1f);
        }

        //Add a button to get the cursor back!
        //gameObject.SetActive(true);

    }

    void clickTimer()
    {
        playerClicked = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
