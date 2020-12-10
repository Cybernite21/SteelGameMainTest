using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshObstacle))]
public class TestObj : Interactable
{
    public float throwForce = 8f;
    public float soundVelocityMultiplier = 0.5f;

    public Vector3 holdOffset;

    public override void Update()
    {
        base.Update();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Input.GetMouseButtonDown(1) && hasInteracted && player.gameObject.GetComponent<PlayerControler>().holding)
        {
            transform.parent = null;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<NavMeshObstacle>().enabled = true;
            player.gameObject.GetComponent<PlayerControler>().holding = false;
            player.gameObject.GetComponent<PlayerControler>().focus = null;
            OnDefocused();
        }
    }

    public override void Interact()
    {
        if(!player.gameObject.GetComponent<PlayerControler>().holding) 
        {
            base.Interact();
            player.gameObject.GetComponent<PlayerMotor>().stopFollowingTarget();
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.transform.position = player.position;
            gameObject.transform.rotation = player.rotation;
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
            //gameObject.transform.position = new Vector3(transform.position.x + holdOffset.x, transform.position.y + holdOffset.y, transform.position.z + holdOffset.z);
            gameObject.transform.localPosition = new Vector3(transform.localPosition.x + holdOffset.x, transform.localPosition.y + holdOffset.y, transform.localPosition.z + holdOffset.z);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;

            player.gameObject.GetComponent<PlayerControler>().holding = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 0.25f && collision.gameObject.tag != "Player")
        {
            GetComponent<AudioSource>().volume = Mathf.Clamp01(collision.relativeVelocity.magnitude * soundVelocityMultiplier);
            GetComponent<AudioSource>().Play(); 
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.relativeVelocity.magnitude <= 0.25f && collision.gameObject.tag != "Player")
        {
            GetComponent<AudioSource>().volume = Mathf.Clamp01(collision.relativeVelocity.magnitude * soundVelocityMultiplier)/3;
            GetComponent<AudioSource>().Play();
        }
    }
}
