using Unity.VisualScripting;
using UnityEngine;

public class HandGrab : MonoBehaviour
{
    [SerializeField]
    private float ThrowForce = 4.0f;

    private ArmRaccoon armRaccoon;

    private bool grabbing = false;
    private Grabbable heldObject = null;
    private Rigidbody objectRigidbody = null;

    private void Awake()
    {
        Transform parent = transform.parent;
        while(parent != null)
        {
            armRaccoon = parent.GetComponent<ArmRaccoon>();
            if (armRaccoon != null)
            {
                break;
            }
            parent = parent.parent;
        }
        if(armRaccoon == null)
        {
            Debug.LogError("No arm raccoon parent found!");
        }
    }

    public void Grab(Grabbable other, Vector3 collisionPoint)
    {
        if(other == null)
        {
            return;
        }
        if (!grabbing)
        {
            return;
        }
        if (heldObject)
        {
            return;
        }
        heldObject = other;
        other.transform.parent = this.transform;
        //other.transform.position = collisionPoint;
        other.gameObject.layer = 3;
        objectRigidbody = other.GetComponent<Rigidbody>();
        if(objectRigidbody)
        {
            objectRigidbody.useGravity = false;
            objectRigidbody.isKinematic = true;
        }
    }

    public void SetGrabbing(bool newGrabbing)
    {
        grabbing = newGrabbing;

        if (heldObject && !grabbing)
        {
            heldObject.gameObject.layer = heldObject.layer;
            heldObject.transform.parent = null;
            if (objectRigidbody)
            {
                objectRigidbody.useGravity = true;
                objectRigidbody.isKinematic = false;
            }
            Rigidbody rigidbody = heldObject.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.AddForceAtPosition(armRaccoon.transform.forward * ThrowForce, rigidbody.transform.position + Vector3.up, ForceMode.Impulse);
            }
            heldObject = null;
            objectRigidbody = null;
        }
    }
}
