using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Key Requirement")]
    [SerializeField] private ItemSO requiredItem;

    private bool isOpen = false;

    public void Interact()
    {
        if (isOpen)
            return;

        Interactor interactor =
            FindFirstObjectByType<Interactor>();

        if (interactor == null)
        {
            Debug.LogError("Door: No Interactor found!");
            return;
        }

        Inventory inventory = interactor.GetInventory();

        if (inventory == null)
        {
            Debug.LogError("Door: Inventory could not be found!");
            return;
        }

        ItemSO equippedItem = inventory.GetEquippedItem();

        // Nothing equipped
        if (equippedItem == null)
        {
            Debug.Log("You aren't holding a key.");
            return;
        }

        // Wrong item
        if (equippedItem != requiredItem)
        {
            Debug.Log(
                "Wrong item! You need " +
                requiredItem.ItemName
            );
            return;
        }

        // Correct item
        Debug.Log("Correct key! Opening door.");

        OpenDoor();

        // Consume one key
        inventory.ConsumeEquippedItem(1);
    }

    private void OpenDoor()
    {
        isOpen = true;

        // Temporary test
        gameObject.SetActive(false);

        Debug.Log("Door opened!");
    }
}