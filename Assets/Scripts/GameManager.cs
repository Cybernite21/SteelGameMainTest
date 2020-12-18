using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused = false;

    public int frameRate = -1;
    public float plrCathTimer = 1f;

    public Text fpsText;
    public Text healthText;

    PlayerControler plrCtrl;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = frameRate;
        Enemy.plrCatchTimer = plrCathTimer;
        //Enemy.plrCaught += restartLevel;
        PlayerControler.dead += restartLevel;
        plrCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsText.text = Mathf.RoundToInt((1f / Time.deltaTime)).ToString();
        healthText.text = "Health: " + plrCtrl.health;

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

    void restartLevel()
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
    }

    //unpause game
    public void unpauseGame()
    {
        Time.timeScale = 1;
        paused = false;
    }
}
