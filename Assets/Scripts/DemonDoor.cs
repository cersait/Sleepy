using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DemonDoor : MonoBehaviour
{
    public Animator Door;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Door.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Door.SetBool("Open", false);
        }
    }
}
