using System.ComponentModel;
using UnityEngine;

public class SuspicionSystem : MonoBehaviour
{

    public static SuspicionSystem Instance { get; private set; }

    [SerializeField]
    public float suspicionLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddSuspicion(float suspicion)
    {
        suspicionLevel += suspicion;
    }
}
