using UnityEngine;

public class RandomItem : MonoBehaviour
{
    [SerializeField] private MonoBehaviour item1;
    [SerializeField] private MonoBehaviour item2;
    [SerializeField] private MonoBehaviour item3;

    private void Start()
    {
        item1.enabled = false;
        item2.enabled = false;
        item3.enabled = false;

        int randomItem = Random.Range(0, 3);

        switch (randomItem)
        {
            case 0:
                item1.enabled = true;
                break;

            case 1:
                item2.enabled = true;
                break;

            case 2:
                item3.enabled = true;
                break;
        }
    }
}