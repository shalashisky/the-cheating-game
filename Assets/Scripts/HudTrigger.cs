using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudTrigger : MonoBehaviour
{
    private HudManager hud;
    private int playerTriggerCount;
    // Update is called once per frame

    private void Start()
    { 
        playerTriggerCount = 0;

        if(gameObject.name=="p1")
        {
            hud = GameObject.Find("PlayerContainer1").GetComponent<HudManager>();
        }
        else if (gameObject.name == "p2")
        {
            hud = GameObject.Find("PlayerContainer2").GetComponent<HudManager>();
        }
        else if (gameObject.name == "p3")
        {
            hud = GameObject.Find("PlayerContainer3").GetComponent<HudManager>();
        }
        else if (gameObject.name == "p4")
        {
            hud = GameObject.Find("PlayerContainer4").GetComponent<HudManager>();
        }
    }
    void Update()
    {
        if (!hud.isDead && playerTriggerCount>0)
            hud.Dim();
        else if (!hud.isDead && playerTriggerCount<=0)
            hud.UnDim();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerTriggerCount+=1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerTriggerCount -= 1;
        }
    }
}
