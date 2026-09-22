using UnityEngine;

public class Opener : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject Opened;
    [SerializeField] private Animator animator;

    private bool isOpen = false;

    public void Interact()
    {
        if (Opened == null || animator == null)
            return;

        isOpen = !isOpen;

        animator.SetBool("Open", isOpen);
    }
}