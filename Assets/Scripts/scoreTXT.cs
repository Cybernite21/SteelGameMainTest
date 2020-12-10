using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreTXT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Home.pointsChanged += updateScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateScore()
    {
        GetComponent<Text>().text = Home.points.ToString();
    }
}
