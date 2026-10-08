using System.Collections.Generic;
using UnityEngine;

public class LightLever : MonoBehaviour, IInteractable
{
    [SerializeField] private LightPuzzle puzzle;

    private static List<int> talPool = new List<int> { 0, 1, 2, 3, 4 };

    public int chosenNumber;

    private void Start() // fixa detta med samma nummer
    {
        // Choose the number ONCE when the game starts
        if (talPool.Count > 0)
        {
            int randomIndex = Random.Range(0, talPool.Count);
            chosenNumber = talPool[randomIndex];
            talPool.RemoveAt(randomIndex);

            Debug.Log(gameObject.name + " chose number: " + chosenNumber); 
        }
    }

    public void Interact()
    {
        if (puzzle == null)
            return;

        // Always use the same number
        puzzle.ToggleLever(chosenNumber);
    }
}