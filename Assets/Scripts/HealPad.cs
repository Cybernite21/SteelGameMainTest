using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPad : MonoBehaviour
{
    public bool healing = false;
    public float delay = 1f;

    PlayerControler plrCtrl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator heal()
    {
        while(healing)
        {
            plrCtrl.health += 2;
            yield return new WaitForSeconds(delay);
            yield return null;
        }
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerControler>(out plrCtrl))
        {
            healing = true;
            StartCoroutine(heal());
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerControler plrCtrl;
        if (other.TryGetComponent<PlayerControler>(out plrCtrl))
        {
            healing = false;
            StopCoroutine(heal());
        }
    }
}
