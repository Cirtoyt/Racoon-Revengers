using UnityEngine;
using UnityEngine.Events;

public class TriggerBox : MonoBehaviour
{
    [SerializeField] UnityEvent triggerEvent;
    bool invoked = false;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision");
        if(collision.gameObject.CompareTag("Player") && !invoked)
        {

            triggerEvent.Invoke();
            invoked = true;
        }
    }
}
