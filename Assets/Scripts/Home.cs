using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    public static event System.Action pointsChanged;

    public static int points = 0;
    public AudioClip gainPoints;
    public AudioClip lostPoints;

    [ColorUsage(true, true)]
    public Color homeColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player" && other.gameObject.tag != "Floor")
        {
            if(other.gameObject.GetComponent<Interactable>().value > 0 && !other.gameObject.GetComponent<Interactable>().gavePoints)
            {
                points += other.gameObject.GetComponent<Interactable>().value;
                GetComponent<AudioSource>().clip = gainPoints;
                GetComponent<AudioSource>().Play();
                other.gameObject.GetComponent<Interactable>().gavePoints = true;
                //other.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
            }
            other.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", homeColor);
        }
        if(pointsChanged != null)
        {
            pointsChanged();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Floor")
        {
            other.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", other.gameObject.GetComponent<Interactable>().originalColor);

            if (other.gameObject.GetComponent<Interactable>().value > 0 && other.gameObject.GetComponent<Interactable>().gavePoints)
            {
                points -= other.gameObject.GetComponent<Interactable>().value;
                GetComponent<AudioSource>().clip = lostPoints;
                GetComponent<AudioSource>().Play();
                other.gameObject.GetComponent<Interactable>().gavePoints = false;
            }
        }
        if (pointsChanged != null)
        {
            pointsChanged();
        }
    }
}
