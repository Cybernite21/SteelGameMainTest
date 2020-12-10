using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int frameRate = -1;
    public float plrCathTimer = 1f;

    public Text fpsText;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = frameRate;
        Enemy.plrCatchTimer = plrCathTimer;
        Enemy.plrCaught += restartLevel;
    }

    // Update is called once per frame
    void Update()
    {
        fpsText.text = Mathf.RoundToInt((1f / Time.deltaTime)).ToString();
    }

    void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Enemy.plrCaught -= restartLevel;
    }
}
