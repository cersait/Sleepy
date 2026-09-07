using UnityEngine;

public class AutoSave : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("AutoSave", 2);
    }

}
