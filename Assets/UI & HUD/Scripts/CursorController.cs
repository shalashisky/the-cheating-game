using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public float cursorSpeed = 4f;
    float moveHorizontalP1;
    bool playerClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        moveHorizontalP1 = Input.GetAxis("MoveHorizontal1");
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D)){
            this.gameObject.transform.Translate(Vector3.right * cursorSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(Vector3.left * cursorSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.gameObject.transform.Translate(Vector3.up * cursorSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.gameObject.transform.Translate(Vector3.down * cursorSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            playerClicked = true;
        }

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
