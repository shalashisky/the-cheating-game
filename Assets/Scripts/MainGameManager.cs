using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MainGameManager : MonoBehaviour
{
    public int numPlayers;

    public GameObject playerPrefab;

    public GameObject[] playerSpawns;
    private int[] playerOrders;

    private GameObject newPlayer;

    // Start is called before the first frame update
    void Start()
    {

        //Make sure player 1 doesn't collide with others
        Physics.IgnoreLayerCollision(9, 10);
        Physics.IgnoreLayerCollision(9, 11);
        Physics.IgnoreLayerCollision(9, 12);

        //Make sure player 2 doesn't collide with others
        Physics.IgnoreLayerCollision(10, 9);
        Physics.IgnoreLayerCollision(10, 11);
        Physics.IgnoreLayerCollision(10, 12);

        //Make sure player 3 doesn't collide with others
        Physics.IgnoreLayerCollision(11, 9);
        Physics.IgnoreLayerCollision(11, 10);
        Physics.IgnoreLayerCollision(11, 12);

        //Make sure player 4 doesn't collide with others
        Physics.IgnoreLayerCollision(12, 9);
        Physics.IgnoreLayerCollision(12, 10);
        Physics.IgnoreLayerCollision(12, 11);

        playerOrders = new int[numPlayers];

        for (int k=0; k<numPlayers;k++)
        {
            playerOrders[k] = k+1;
        }

        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //I feel like a highschooler for unironically doing this...
    private void ShuffleOrder()
    {
        for (int j=0; j<playerOrders.Length;j++)
        {
            int tmp = playerOrders[j];
            int r = Random.Range(j, playerOrders.Length);
            playerOrders[j] = playerOrders[r];
            playerOrders[r] = tmp;
        }
    }

    private void SpawnPlayers()
    {
        ShuffleOrder();

        for (int i=0; i<numPlayers; i++)
        {
            newPlayer = Instantiate(playerPrefab, playerSpawns[i].transform.position, Quaternion.identity);
            newPlayer.GetComponent<PlayerInput>().playerNumber = playerOrders[i];
            newPlayer.GetComponent<PlayerController>().playerNumber = playerOrders[i];

        }
    }
}
