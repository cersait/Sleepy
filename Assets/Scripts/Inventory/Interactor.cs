using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 3f;
    public GameObject interactUI;

    [Header("Player References")]
    [SerializeField] private Inventory inventory;

    private IInteractable currentInteractable;

    private void Awake()
    {
        // Try to find Inventory automatically
        if (inventory == null)
        {
            inventory = GetComponentInParent<Inventory>();
        }

        if (inventory == null)
        {
            Debug.LogError(
                "Interactor: Inventory could not be found. " +
                "Make sure Inventory is on the Player or assign it in the Inspector."
            );
        }
    }

    private void Start()
    {
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }

    private void Update()
    {
        DetectInteractable();

        if (currentInteractable != null &&
            Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    private void DetectInteractable()
    {
        currentInteractable = null;

        if (Camera.main == null)
            return;

        Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactRange))
        {
            // Check object itself
            currentInteractable =
                hit.collider.GetComponent<IInteractable>();

            // Check parent if not found
            if (currentInteractable == null)
            {
                currentInteractable =
                    hit.collider.GetComponentInParent<IInteractable>();
            }

            if (currentInteractable != null)
            {
                if (interactUI != null)
                    interactUI.SetActive(true);

                return;
            }
        }

        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}