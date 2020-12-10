using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayer : MonoBehaviour
{
    public GameObject plr;

    // Start is called before the first frame update
    void Start()
    {
        plr.GetComponent<PlayerControler>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(plr.GetComponent<PlayerControler>().enabled == true)
            plr.GetComponent<PlayerControler>().enabled = false;
    }
}
