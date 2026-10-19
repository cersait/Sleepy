using UnityEngine;

public class BookPull : MonoBehaviour, IInteractable
{
    [SerializeField] private BookPuzzle puzzle;

    [Range(1, 55)]
    [SerializeField] private int bookNumber = 1;

    public void Interact()
    {
        if (puzzle == null)
            return;

        puzzle.ToggleBook(bookNumber);
    }
}