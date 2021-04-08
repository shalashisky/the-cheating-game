using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerInput : MonoBehaviour
{
    public int playerNumber;
    private string inputPath;

    public string leftAnalogX;
    public string leftAnalogY;
    public string rightAnalogX;
    public string rightAnalogY;
    public string jump;
    public string jumpAlt;
    public string fire;
    public string fireAlt;
    public string interact;
    public string taunt;

    private void Start()
    {
        inputPath = "Assets/json files/input.json";
        SelectPlayer(playerNumber);
    }

    private void SelectPlayer(int playerNum)
    {
        
    }
}
