using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DemonDoor : MonoBehaviour
{
    public GameObject Demon;
    public Animator Door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Demon.CompareTag("Enemy"))
        {
            Door.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Demon.CompareTag("Enemy"))
        {
            Door.SetBool("Open", false);
        }
    }
}
