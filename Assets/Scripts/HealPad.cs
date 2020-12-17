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
            plrCtrl.health = Mathf.Clamp(plrCtrl.health, 0, 100);
            yield return new WaitForSecondsRealtime(delay);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerControler>(out plrCtrl))
        {
            if (!healing)
            {
                healing = true;
                StartCoroutine(heal());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerControler plrCtrl;
        if (other.gameObject.TryGetComponent<PlayerControler>(out plrCtrl))
        {
            healing = false;
            StopCoroutine(heal());
        }
    }
}
