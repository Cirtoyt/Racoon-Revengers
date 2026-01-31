using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HandGrab handGrab = other.GetComponent<HandGrab>();
        if (handGrab)
        {
            handGrab.Grab(gameObject);
        }
    }
}
