using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class PlayerInput : MonoBehaviour
{
    public int playerNumber;
    public TextAsset[] inputPath;

    [Serializable]
    public class ControllerInput
    {
        public string lAnalogX;
        public string lAnalogY;
        public string rAnalogX;
        public string rAnalogY;
        public string jump;
        public string jumpAlt;
        public string fire;
        public string fireAlt;
        public string interact;
        public string taunt;
    }

    [SerializeField] 
    public ControllerInput controller = new ControllerInput();

    private void Start()
    {
        GetComponent<PlayerController>().playerNumber = playerNumber;
        Debug.Log(playerNumber);
        SelectPlayerInput(playerNumber);
    }

    private void SelectPlayerInput(int playerNum)
    {
        controller = JsonUtility.FromJson<ControllerInput>(inputPath[playerNumber-1].text);
    }
}
