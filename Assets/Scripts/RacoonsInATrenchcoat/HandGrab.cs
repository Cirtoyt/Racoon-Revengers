using UnityEngine;

public class HandGrab : MonoBehaviour
{
    private bool grabbing = false;
    private GameObject heldObject = null;
    private Rigidbody objectRigidbody = null;

    public void Grab(GameObject other)
    {
        if (!grabbing)
        {
            return;
        }
        if (heldObject)
        {
            return;
        }
        if(other.GetComponent<Grabbable>())
        {
            heldObject = other;
            other.transform.parent = this.transform;
            objectRigidbody = other.GetComponent<Rigidbody>();
            if(objectRigidbody)
            {
                objectRigidbody.useGravity = false;
                objectRigidbody.isKinematic = true;
            }
        }
    }

    public void SetGrabbing(bool newGrabbing)
    {
        grabbing = newGrabbing;

        if (heldObject && !grabbing)
        {
            heldObject.transform.parent = null;
            if (objectRigidbody)
            {
                objectRigidbody.useGravity = true;
                objectRigidbody.isKinematic = false;
            }
            heldObject = null;
            objectRigidbody = null;
        }
    }
}
