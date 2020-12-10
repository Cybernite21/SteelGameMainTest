using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float radius = 3;
    public Transform interactionTransform;

    public bool isFocus = false;
    public Transform player;

    public bool hasInteracted = false;

    float distance;

    public Color originalColor;
    public int value = 100;
    public bool gavePoints = false;

    void Start()
    {
        originalColor = GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    public virtual void Interact()
    {
        //This method is meant to be overwriten
        print("Interacting with:" + " " + gameObject);
    }

    public virtual void Update()
    {
        if (isFocus && !hasInteracted)
        {
            distance = Vector3.Distance(player.transform.position, interactionTransform.position);
            
            if(distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    void OnDrawGizmos()
    {
        if (interactionTransform == null)
            interactionTransform = transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
