using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField]
    private float suspicionAdded = 1.0f;

    private SuspicionSystem suspicionSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        suspicionSystem = SuspicionSystem.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Floor>())
        {
            SuspicionSystem.Instance.AddSuspicion(suspicionAdded);
        }
    }
}
