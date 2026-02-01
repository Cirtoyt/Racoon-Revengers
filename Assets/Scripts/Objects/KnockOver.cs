using UnityEngine;
using UnityEngine.Events;

public class KnockOver : MonoBehaviour
{
    private bool fallen = false;
    [SerializeField] UnityEvent knockEvent;

    private void OnTriggerEnter(Collider collision)
    {
        if (!fallen)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                //increase suspicion bar
                //trigger a dialogue
                knockEvent.Invoke();
                fallen = true;
            }
        }
    }
}
