using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TaskAssignment : MonoBehaviour
{
    //Dialogue
    [SerializeField] private TextMeshProUGUI subtitles;
    [SerializeField] private List<string> dialogue;
    private int currentIndex;

    //Movement
    private NavMeshAgent agent;
    [SerializeField] private List<GameObject> waypoints;
    private int currentWaypoint;

    //Canvas
    [SerializeField] private TextMeshProUGUI task;
    [SerializeField] private RawImage textBox;
    [SerializeField] private Animator textboxAnim;

    //Tasks
    [SerializeField] private List<string> taskTexts;
    private int currentTask;

    private void Start()
    {
        subtitles.text = dialogue[0];
        currentIndex = 0;
        currentWaypoint = 0;
        currentTask = 0;

        agent = GetComponent<NavMeshAgent>();
        transform.position = waypoints[0].transform.position;
        agent.updatePosition = true;
    }
    public void StartDialogue()
    {
        subtitles.text = dialogue[0];
    }

    public void AdvanceText()
    {
        subtitles.text = dialogue[currentIndex + 1];
        currentIndex++;
    }

    public void NextWaypoint()
    {
        agent.SetDestination(waypoints[currentWaypoint + 1].transform.position);
        currentWaypoint++;
    }

    public void SetTask()
    {
        task.text = taskTexts[currentTask];
        currentTask++;
    }

}
