using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuspicionSystem : MonoBehaviour
{

    public static SuspicionSystem Instance { get; private set; }

    [SerializeField] private float objectiveSusTimerDuration = 30;
    [SerializeField] private float objectiveSusIncreaseAmount = 0.3f;
    [SerializeField] private AudioSource AhRaccoonsSound;

    private float suspicionLevel = 0;
    private bool objectiveTimerRunning = false;
    private float objectiveTimer = 0;

    [SerializeField] TaskAssignment joe;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameplayUIManager.Instance.SetSuspicionValue(suspicionLevel);
    }

    private void Update()
    {
        if (objectiveTimerRunning)
        {
            objectiveTimer += Time.deltaTime;
            if (objectiveTimer >= objectiveSusTimerDuration)
            {
                AddSuspicion(objectiveSusIncreaseAmount);
                objectiveTimer = 0;


                Debug.Log("Objective taking too long! Sus being added!!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(3);
    }

    public void AddSuspicion(float suspicion)
    {
        suspicionLevel += suspicion;

        GameplayUIManager.Instance.SetSuspicionValue(suspicionLevel);

        // Trigger sound cue based on how sus we are

        if (suspicionLevel >= 1)
        {
            // End Game
            // Ah! Raccooons!
            AhRaccoonsSound.Play();
            RaccoonsInATrenchcoatManager.Instance.FallApart();
        }
    }

    /// <summary>
    /// Resets the timer if called whilst it is running
    /// </summary>
    public void BeginObjectiveTimer()
    {
        objectiveTimer = 0;
        objectiveTimerRunning = true;

        Debug.Log("Reset and began objective timer");
    }

    /// <summary>
    /// Call when ending game
    /// </summary>
    public void EndObjectiveTimer()
    {
        objectiveTimer = 0;
        objectiveTimerRunning = false;

        Debug.Log("Ended objective timer");
    }
}
