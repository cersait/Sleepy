using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public ItemSO keyItem;
    public ItemSO axeItem;

    public GameObject hotbarObj;
    public GameObject inventorySlotParent;
    public GameObject container;

    public Image dragIcon;

    public float pickupRange = 5f;
    private Item lookedAtItem = null;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lookedAtRenderer = null;

    private int equipeedHotbarIndex = 0; //0-5
    public float equippedOpacity = 0.9f;
    public float normalOpacity = 0.58f;
    public Transform hand;
    private GameObject currentHandItem;

    public GameObject itemDescriptionParent;
    public Image itemDescriptionImage;
    public TextMeshProUGUI descriptionItemNameTxt;
    public TextMeshProUGUI itemDescriptionTxt;

    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private Slot draggedSlot = null;
    private bool isDragging = false;

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<Slot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            container.SetActive(!container.activeInHierarchy);
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
            Look.instance.updatingRotation = !Look.instance.updatingRotation;
        }

        DetectLookedAtItem();
        Pickup();

        StartDrag();
        UpdateDragItemPosition();
        EndDrag();

        HandleHotBarSelection();
        HandleDropEquippedItem();
        UpdateHotbarOpacity();

        UpdateItemDescription();
    }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;

        foreach (Slot slot in hotbarSlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;

                    int amountToAdd = Mathf.Min(
                        spaceLeft,
                        remaining
                    );

                    slot.SetItem(
                        itemToAdd,
                        currentAmount + amountToAdd
                    );

                    remaining -= amountToAdd;

                    if (remaining <= 0)
                        return;
                }
            }
        }

        foreach (Slot slot in hotbarSlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(
                    itemToAdd.maxStackSize,
                    remaining
                );

                slot.SetItem(itemToAdd, amountToPlace);

                remaining -= amountToPlace;

                UpdateHeldItem();

                if (remaining <= 0)
                    return;
            }
        }

        foreach (Slot slot in inventorySlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;

                    int amountToAdd = Mathf.Min(
                        spaceLeft,
                        remaining
                    );

                    slot.SetItem(
                        itemToAdd,
                        currentAmount + amountToAdd
                    );

                    remaining -= amountToAdd;

                    if (remaining <= 0)
                        return;
                }
            }
        }

        foreach (Slot slot in inventorySlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(
                    itemToAdd.maxStackSize,
                    remaining
                );

                slot.SetItem(
                    itemToAdd,
                    amountToPlace
                );

                remaining -= amountToPlace;

                if (remaining <= 0)
                    return;
            }
        }

        if (remaining > 0)
        {
            Debug.Log(
                "Inventory is full, could not add " +
                remaining +
                " of " +
                itemToAdd.ItemName
            );
        }
    }

    private void StartDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Slot hovered = GetHoveredSlot();

            if (hovered != null && hovered.HasItem())
            {
                draggedSlot = hovered;
                isDragging = true;

                //Show drag item
                dragIcon.sprite = hovered.GetItem().icon;
                dragIcon.color = new Color(1, 1, 1, 0.5f);
                dragIcon.enabled = true;
            }
        }
    }

    private void EndDrag()
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Slot hovered = GetHoveredSlot();

            if (hovered != null)
            {
                HandleDrop(draggedSlot, hovered);

                dragIcon.enabled = false;

                draggedSlot = null;
                isDragging = false;
            }
        }
    }

    private Slot GetHoveredSlot()
    {
        foreach (Slot s in allSlots)
        {
            if (s.hovering)
                return s;   
        }

        return null;
    }

    private void HandleDrop(Slot from, Slot to)
    {
        if (from == to) return;

        //Stacking
        if (to.HasItem() && to.GetItem() == from.GetItem())
        {
            int max = to.GetItem().maxStackSize;
            int space = max - to.GetAmount();

            if (space > 0)
            {
                int move = Mathf.Min(space, from.GetAmount());

                to.SetItem(to.GetItem(), to.GetAmount() + move);
                from.SetItem(from.GetItem(), from.GetAmount() - move);

                if (from.GetAmount() <= 0)
                    from.ClearSlot();

                return;
            }
        }

        //Different Item
        if (to.HasItem())
        {
            ItemSO tempItem = to.GetItem();
            int tempAmount = to.GetAmount();

            to.SetItem(from.GetItem(), from.GetAmount());
            from.SetItem(tempItem, tempAmount);
            return;
        }

        //Empty Slot
        to.SetItem(from.GetItem(), from.GetAmount());
        from.ClearSlot();
    }

    private void UpdateDragItemPosition()
    {
        if (isDragging)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    private void Pickup()
    {
        if (lookedAtRenderer != null && Input.GetKeyDown(KeyCode.E))
        {
            Item item = lookedAtRenderer.GetComponent<Item>();
            if (item != null)
            {
                AddItem(item.item, item.amount);
                Destroy(item.gameObject);
            }
        }
    }

    private void DetectLookedAtItem()
    {
        if (lookedAtRenderer != null)
        {
            lookedAtRenderer.material = originalMaterial;
            lookedAtRenderer = null;
            originalMaterial = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                Renderer rend = item.GetComponent<Renderer>();
                if (rend != null)
                {
                    originalMaterial = rend.material;
                    rend.material = highlightMaterial;
                    lookedAtRenderer = rend;
                }
            }
        }
    }

    private void UpdateHotbarOpacity()
    {
        for(int i = 0; i < hotbarSlots.Count; i++)
        {
            Image icon = hotbarSlots[i].GetComponent<Image>();
            if (icon != null)
            {
                icon.color = (i == equipeedHotbarIndex) ? new Color(1, 1, 1, equippedOpacity) : new Color(1, 1, 1, normalOpacity);
            }
        }
    }

    private void HandleHotBarSelection()
    {
        for (int i = 0; i < hotbarSlots.Count && i < 6; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                equipeedHotbarIndex = i;

                UpdateHotbarOpacity();
                UpdateHeldItem();
            }
        }
    }

    private void HandleDropEquippedItem()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;
        {
            Slot equippedSlot = hotbarSlots[equipeedHotbarIndex];

            if (!equippedSlot.HasItem()) return;

            ItemSO itemSO = equippedSlot.GetItem();
            GameObject prefab = itemSO.ItemPrefab;

            if (prefab == null) return;

            GameObject dropped = Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);

            Item item = dropped.GetComponent<Item>();
            item.item = itemSO;
            item.amount = equippedSlot.GetAmount();

            equippedSlot.ClearSlot();
        }
    }

    private void UpdateItemDescription()
    {
        Slot hoveredSlot = GetHoveredSlot();

        if (hoveredSlot != null)
        {
            ItemSO hoveredItem = hoveredSlot.GetItem();

            if (hoveredItem != null)
            {
                itemDescriptionParent.SetActive(true);
                itemDescriptionImage.sprite = hoveredItem.icon;
                itemDescriptionTxt.text = hoveredItem.description;
                descriptionItemNameTxt.text = hoveredItem.name;
                return;
            }
        }
        itemDescriptionParent.SetActive(false);
    }

    public ItemSO GetEquippedItem()
    {
        if (equipeedHotbarIndex < 0 ||
            equipeedHotbarIndex >= hotbarSlots.Count)
        {
            return null;
        }

        Slot slot = hotbarSlots[equipeedHotbarIndex];

        if (!slot.HasItem())
            return null;

        return slot.GetItem();
    }

    public void ConsumeEquippedItem(int amount = 1)
    {
        if (equipeedHotbarIndex < 0 ||
            equipeedHotbarIndex >= hotbarSlots.Count)
        {
            return;
        }

        Slot slot = hotbarSlots[equipeedHotbarIndex];

        if (!slot.HasItem())
            return;

        int newAmount = slot.GetAmount() - amount;

        if (newAmount <= 0)
        {
            // Remove the item from the hotbar
            slot.ClearSlot();

            // Remove the item from the player's hand
            if (currentHandItem != null)
            {
                Destroy(currentHandItem);
                currentHandItem = null;
            }
        }
        else
        {
            // Just reduce the stack
            slot.SetItem(slot.GetItem(), newAmount);
        }
    }

    private void UpdateHeldItem()
    {
        // Remove currently held item
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            currentHandItem = null;
        }

        // Make sure the index is valid
        if (equipeedHotbarIndex < 0 ||
            equipeedHotbarIndex >= hotbarSlots.Count)
        {
            return;
        }

        Slot slot = hotbarSlots[equipeedHotbarIndex];

        // Nothing in the selected slot
        if (!slot.HasItem())
            return;

        ItemSO item = slot.GetItem();

        // Make sure the ItemSO has a prefab
        if (item.ItemPrefab == null)
        {
            Debug.LogWarning(
                item.ItemName + " does not have an ItemPrefab!"
            );
            return;
        }

        // Spawn the item in the player's hand
        currentHandItem = Instantiate(item.ItemPrefab, hand);

        // Reset local position/rotation
        currentHandItem.transform.localPosition = Vector3.zero;
        currentHandItem.transform.localRotation = Quaternion.identity;
        currentHandItem.transform.localScale = Vector3.one;
    }

}
