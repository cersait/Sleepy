using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public ItemSO item;
    public int amount = 1;

    public void Interact()
    {
        Interactor interactor =
        FindFirstObjectByType<Interactor>();
    }

}
