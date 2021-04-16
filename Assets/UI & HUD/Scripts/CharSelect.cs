using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharSelect : MonoBehaviour
{

    public string characterName;

    public Texture[] portraitArray;
    // 0 - nothing
    // 1 - Jerry
    // 2 - Richard

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CursorP1"))
        {
            Debug.Log("Clicked");
            CharacterSelected(1);
            //collision.gameObject.SetActive(false);
            // Later add collisions for tags 2,3,4
        }
    }

    private void CharacterSelected(int playerNum)
    {
        if(characterName == "Jerry Canne")
        {
        RawImage port = GameObject.Find("PortraitP" + playerNum).GetComponent<RawImage>();
            port.texture = portraitArray[1];
        //Sets the players name to the characters name
        TextMeshProUGUI name = GameObject.Find("CharNameP" + playerNum).GetComponent<TextMeshProUGUI>();
            name.text = characterName;

        }
        else if (characterName == "Henry Brickston")
        {
            RawImage port = GameObject.Find("PortraitP" + playerNum).GetComponent<RawImage>();
            port.texture = portraitArray[2];
            //Sets the players name to the characters name
            TextMeshProUGUI name = GameObject.Find("CharNameP" + playerNum).GetComponent<TextMeshProUGUI>();
            name.text = characterName; // Turn into function
        }
        else if (characterName == "Cheri Bombe")
        {
            RawImage port = GameObject.Find("PortraitP" + playerNum).GetComponent<RawImage>();
            port.texture = portraitArray[3];
            //Sets the players name to the characters name
            TextMeshProUGUI name = GameObject.Find("CharNameP" + playerNum).GetComponent<TextMeshProUGUI>();
            name.text = characterName; // Turn into function
        }
        else if (characterName == "Jordan Wardenson")
        {
            RawImage port = GameObject.Find("PortraitP" + playerNum).GetComponent<RawImage>();
            port.texture = portraitArray[4];
            //Sets the players name to the characters name
            TextMeshProUGUI name = GameObject.Find("CharNameP" + playerNum).GetComponent<TextMeshProUGUI>();
            name.text = characterName; // Turn into function
        }
        else if (characterName == "Locked")
        {
            //Add a sound effect
        }
    } 


}
