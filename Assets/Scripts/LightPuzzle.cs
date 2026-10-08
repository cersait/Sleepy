using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;

public class LightPuzzle : MonoBehaviour
{
    [Header("The 5 levers")]
    [SerializeField] private GameObject[] levers = new GameObject[5];

    [Header("The lights for each lever")]
    [SerializeField] private Light[] lights = new Light[5];

    [Header("Door")]
    [SerializeField] private Animator doorAnimator;

    [SerializeField] private int requiredLevers = 3;

    private bool[] leverStates = new bool[5];
    private bool puzzleSolved = false;

    private void Start()
    { 
        for (int i = 0; i < lights.Length; i++) 
        { 
            if (lights[i] != null) 
                lights[i].enabled = false; 
        } 
    }
    public void ToggleLever(int index)
    {
        if (puzzleSolved)
            return;

        if (index < 0 || index >= leverStates.Length)
            return;

        // Toggle this lever
        leverStates[index] = !leverStates[index];

        // Turn its light on/off
        if (lights[index] != null)
            lights[index].enabled = leverStates[index];

        CheckPuzzle();
    }

    private void CheckPuzzle()
    {
        int enabledCount = 0;

        for (int i = 0; i < leverStates.Length; i++)
        {
            if (leverStates[i])
                enabledCount++;
        }

        Debug.Log("Levers enabled: " + enabledCount + "/5");

        if (enabledCount >= requiredLevers)
        {
            SolvePuzzle();
        }
    }

    private void SolvePuzzle()
    {
        puzzleSolved = true;

        Debug.Log("LIGHT PUZZLE SOLVED!");

        if (doorAnimator != null)
        {
            doorAnimator.SetBool("Open", true);
        }
    }

    public bool IsLeverEnabled(int index)
    {
        if (index < 0 || index >= leverStates.Length)
            return false;

        return leverStates[index];
    }
}
