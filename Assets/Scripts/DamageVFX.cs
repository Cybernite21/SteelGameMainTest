using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageVFX : MonoBehaviour
{
    PlayerControler plr;
    Color clr;
    // Start is called before the first frame update
    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        clr = gameObject.GetComponent<RawImage>().color;
    }

    // Update is called once per frame
    void Update()
    {
        clr = new Color(clr.r, clr.g, clr.b, Mathf.Lerp(1, 0, (float)plr.health / 100f));
        gameObject.GetComponent<RawImage>().color = clr;
    }
}
