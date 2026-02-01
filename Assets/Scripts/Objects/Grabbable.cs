using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public int layer { get; private set; }

    private void Start()
    {
        layer = gameObject.layer;
    }

    private void OnTriggerEnter(Collider other)
    {
        HandGrab handGrab = other.GetComponent<HandGrab>();

        Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);

        if (handGrab)
        {
            handGrab.Grab(this, collisionPoint);
        }
    }
}
