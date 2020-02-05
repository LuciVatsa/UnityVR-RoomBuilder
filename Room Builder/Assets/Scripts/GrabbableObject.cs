using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GrabbableObject : MonoBehaviour
{
    private Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void HandHoverUpdate(Hand hand)
    {
        if(interactable == null)
        {
            Debug.Log("Object Cant Be Grabbbed");
        }
        else
        {
            GrabTypes grabType = hand.GetGrabStarting();
            bool bGrabEnding = hand.IsGrabEnding(gameObject);
            //Start Grabbing
            if(interactable.attachedToHand == null && grabType!= GrabTypes.None)
            {
                hand.AttachObject(gameObject, grabType);
                hand.HoverLock(interactable);
            }
            else if(bGrabEnding)
            {
                hand.DetachObject(gameObject);
                hand.HoverUnlock(interactable);
            }
        }
    }
}
