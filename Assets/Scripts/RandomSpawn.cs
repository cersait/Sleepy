using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    private static List<int> talPool = new List<int> { 1, 2, 3 };

    public GameObject Crate;

    [Header("Possible locations")]
    [SerializeField] private Transform place1;
    [SerializeField] private Transform place2;
    [SerializeField] private Transform place3;

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
                    Crate.transform.position = place1.transform.position;
                    break;

                case 2:
                    Crate.transform.position = place2.transform.position;
                    break;

                case 3:
                    Crate.transform.position = place3.transform.position;
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