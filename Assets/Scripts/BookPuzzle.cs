using UnityEngine;

public class BookPuzzle : MonoBehaviour
{
    [Header("10 Books")]
    [SerializeField] private GameObject[] books = new GameObject[10];

    [Header("Book Animators")]
    [SerializeField] private Animator[] bookAnimators = new Animator[10];

    [Header("Door")]
    [SerializeField] private Animator doorAnimator;

    [Header("Correct Book Numbers")]
    [SerializeField] private int[] correctBooks = { 1, 3, 5 };

    private bool[] bookStates = new bool[55];
    private bool puzzleSolved = false;

    public void ToggleBook(int bookNumber)
    {
        if (puzzleSolved)
            return;

        // Book numbers are 1-10, array indexes are 0-9
        int index = bookNumber - 1;

        if (index < 0 || index >= bookStates.Length)
            return;

        // Toggle the selected book
        bookStates[index] = !bookStates[index];

        // Play this book's own animation
        if (bookAnimators[index] != null)
        {
            bookAnimators[index].SetBool("Moved", bookStates[index]);
        }

        CheckPuzzle();
    }

    private void CheckPuzzle()
    {
        // Every correct book must be enabled
        foreach (int bookNumber in correctBooks)
        {
            int index = bookNumber - 1;

            if (index < 0 || index >= bookStates.Length)
                return;

            if (!bookStates[index])
                return;
        }

        PuzzleSolved();
    }

    private void PuzzleSolved()
    {
        puzzleSolved = true;

        Debug.Log("BOOK PUZZLE SOLVED!");

        if (doorAnimator != null)
        {
            doorAnimator.SetBool("Open", true);
        }
    }

    public bool IsBookEnabled(int bookNumber)
    {
        int index = bookNumber - 1;

        if (index < 0 || index >= bookStates.Length)
            return false;

        return bookStates[index];
    }
}