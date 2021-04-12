using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI titleText;
    private GameObject panel;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        panel = GameObject.Find("Panel");
        player = GameObject.Find("Player");
        gameOverText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (player.GetComponent<PlayerControllerTwinstick>().isDead)
            GameOver();
    }

    // Update is called once per frame
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("fuck you");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        titleText.gameObject.SetActive(false);
        panel.SetActive(false);
    }
}
