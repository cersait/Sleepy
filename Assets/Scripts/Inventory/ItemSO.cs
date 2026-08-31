using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "NewItem")]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public Sprite icon;
    public int maxStackSize;
    public GameObject ItemPrefab;
    public GameObject handItemPrefab;
    public string description;
}
