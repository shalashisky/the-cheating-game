using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    //Find the name entry textMeshPro
    public TextMeshProUGUI nameEntry;

    //Button Press Timer
    float keyTimerMax = 5f; // Seconds

    public float currentTime = 0f;

    public string[] letterArray;

    public string playerName;

    public string currentLetter;

    public Button curButton;

    public int i = 0; // BAD FIX LATER

    public bool initialPress = true;

    public bool timeToUpdate = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        //Always counting down
        //Button script with timer
        currentTime -= Time.deltaTime;

        if(currentTime < 0)
        {
            currentTime = 0;
        }

        if (currentTime == 0)
        {
            initialPress = true;
            UpdatePlayerName();
        }
        else
        {
            initialPress = false;
        }

    }

    public void AssignPlayer(int playerNum)
    {

        if (playerNum == 1)
        {
            nameEntry = GameObject.Find("NameEntryP1").GetComponent<TextMeshProUGUI>();
        }
        else if (playerNum == 2)
        {
            nameEntry = GameObject.Find("NameEntryP2").GetComponent<TextMeshProUGUI>();
        }
        else if (playerNum == 3)
        {
            nameEntry = GameObject.Find("NameEntryP3").GetComponent<TextMeshProUGUI>();
        }
        else if (playerNum == 4)
        {
            nameEntry = GameObject.Find("NameEntryP4").GetComponent<TextMeshProUGUI>();
        }
    }

    //User presses button in game and then this function runs.

    public void KeyPress(string letterGroup)
    {
            //Make sure gameobjects are named right // Change 1 to name entry
            //curButton = GameObject.Find(letterGroup + 1).GetComponent<Button>();
            if (letterGroup == "ABC")
            {
            KeyHandler(0, 5);
        } else if (letterGroup == "DEF")
        {
            KeyHandler(6, 11);
        }
    }

   
    void KeyHandler(int min, int max) // take the min and max range for the index of the set group. EX ABCabc is min 0 - 6
    {

        timeToUpdate = true;


        if (initialPress == true) // first time clicking the button sets it to the first value;
        {
            i = min; // sets the value to the first value on click
            currentLetter = letterArray[i];
            initialPress = false;
            currentTime = keyTimerMax;
            Debug.Log("First");
        }
        else if (!initialPress)
        {
            i++; //cycle through letters
            currentLetter = letterArray[i];
            currentTime = keyTimerMax;
            Debug.Log("Second");
        }

        if (!initialPress && i >= max)
        {
            i = min; // reset index and loop letters
            currentLetter = letterArray[i];
            currentTime = keyTimerMax;
            nameEntry.text += currentLetter;
            Debug.Log("Third");
        }

        nameEntry.text = playerName + currentLetter;

        Debug.Log(i);

        //if on click button again and timer is not 0 then letterArray Index ++, if letter index hits max letters then loop back
    }

  
    void UpdatePlayerName()
    {
        if (timeToUpdate) { 
        playerName += currentLetter;
        nameEntry.text = playerName;
        timeToUpdate = false;
        }
    }


    //I have to have two function because of how buttons work
    //Check the player number to assign the correct nameEntry GO






}
