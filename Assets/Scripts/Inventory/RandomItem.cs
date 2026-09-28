using System.Collections.Generic;
using UnityEngine;

public class RandomItem: MonoBehaviour
{
    private static List<int> talPool = new List<int> {1, 1, 2, 2, 3};

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

        if (talPool.Count > 0)
        {
            int randomIndex = Random.Range(0, talPool.Count);
            

           

            switch (talPool[randomIndex])
            {
                case 1:
                    item.item = item1;
                    break;

                case 2:
                    item.item = item2;
                    break;

                case 3:
                    item.item = item3;
                    break;
            }
            talPool.RemoveAt(randomIndex);

            string test = "";
            for (int i = 0; i < talPool.Count; i++)
            {
                test += talPool[i] + ", ";
            }
            print(gameObject.name + ": " + test);
            Debug.Log("This object is: " + item.item.ItemName);
        }

        
    }
}