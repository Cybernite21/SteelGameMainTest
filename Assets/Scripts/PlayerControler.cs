using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerControler : MonoBehaviour
{
    public Interactable focus;

    Camera cam;
    PlayerMotor motor;

    public LayerMask movementMask;
    public LayerMask objectsMask;

    public bool holding = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Debug.Log("We Hit" + hit.collider.name + " " + hit.point);
                motor.moveToPoint(hit.point);

                //Move Player to Mouse Click

                //Stop focusing
                if(!holding)
                    removeFocus();
            }
        }

        if (Input.GetMouseButtonDown(1) && !holding)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200, objectsMask))
            {
                //check if we hit a interactable
                //if we did set it as out focus
                Interactable interactable =  hit.collider.gameObject.GetComponent<Interactable>();

                if(interactable != null)
                {
                    setFocus(interactable);
                }
            }
        }
    }

    void setFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if(focus != null)
                focus.OnDefocused();
            focus = newFocus;
            focus.OnFocused(transform);
        }

        motor.followTarget(newFocus);
    }

    void removeFocus()
    {
        if(focus != null)
            focus.OnDefocused();
        focus = null;
        motor.stopFollowingTarget();
    }
}
