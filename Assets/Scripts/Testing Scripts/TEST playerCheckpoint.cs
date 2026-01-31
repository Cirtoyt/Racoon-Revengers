using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TESTplayerCheckpoint : MonoBehaviour
{
    [SerializeField] private List<GameObject> PlayerObjectives;
    private int currentObjective;

    private int index = 0;

    private void Start()
    {
        currentObjective = 0;
        SetActiveObjectives();        
    }

    public void NextObjective()
    {
        currentObjective++;

        Debug.Log("Current objective: " + currentObjective);
        SetActiveObjectives();
    }

    private void SetActiveObjectives()
    {
        index = 0;

        foreach (var objective in PlayerObjectives)
        {
            if (index == currentObjective)
            {
                Debug.Log("index: " + index);
                objective.SetActive(true);
            }
            else
            {
                Debug.Log("setting inactive");
                objective.SetActive(false);
            }

            index++;
        }
        
    }
}
