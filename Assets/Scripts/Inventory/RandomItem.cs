using UnityEngine;

public class RandomItem: MonoBehaviour
{
    [Header("Possible Items")]
    [SerializeField] private ItemSO item1;
    [SerializeField] private ItemSO item2;
    [SerializeField] private ItemSO item3;

    private void Start()
    {
        Item item = GetComponent<Item>();

        if (item == null)
        {
            Debug.LogError("RandomItemSelector needs an Item component!");
            return;
        }

        int randomIndex = Random.Range(0, 3);

        switch (randomIndex)
        {
            case 0:
                item.item = item1;
                break;

            case 1:
                item.item = item2;
                break;

            case 2:
                item.item = item3;
                break;
        }

        Debug.Log("This object is: " + item.item.ItemName);
    }
}