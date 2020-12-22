using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static RuntimePlatform currentPlatform;

    public bool paused = false;

    public GameObject pausePanel;
    public GameObject wonPanel;
    public GameObject[] objects;

    public int frameRate = -1;
    public float plrCathTimer = 1f;
    int pointsToWin = 0;

    public Text fpsText;
    public Text healthText;
    public Text pointsText;

    PlayerControler plrCtrl;

    //Called when script is being loaded
    void Awake()
    {
        currentPlatform = Application.platform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = frameRate;
        Enemy.plrCatchTimer = plrCathTimer;
        //Enemy.plrCaught += restartLevel;
        PlayerControler.dead += restartLevel;
        plrCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        objects = GameObject.FindGameObjectsWithTag("object");
        foreach(GameObject obj in objects)
        {
            pointsToWin += obj.GetComponent<TestObj>().value;
        }
        unpauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        fpsText.text = Mathf.RoundToInt((1f / Time.deltaTime)).ToString();
        healthText.text = "Health: " + plrCtrl.health;
        pointsText.text = Home.points.ToString();

        //Check if player won
        if(Home.points >= pointsToWin)
        {
            won();
        }

        //Pause or unpause game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                unpauseGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerControler.dead -= restartLevel;
        //Enemy.plrCaught -= restartLevel;
    }

    //pause game
    public void pauseGame()
    {
        Time.timeScale = 0;
        paused = true;
        pausePanel.SetActive(true);
    }

    public void menu()
    {
        SceneManager.LoadScene(0);
    }

    //unpause game
    public void unpauseGame()
    {
        Time.timeScale = 1;
        paused = false;
        pausePanel.SetActive(false);
    }

    public void won()
    {
        unpauseGame();
        Time.timeScale = 0;
        paused = true;
        wonPanel.SetActive(true);
    }

    public void quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
