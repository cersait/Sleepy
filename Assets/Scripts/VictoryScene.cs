using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("Victory");
        }
    }
}
