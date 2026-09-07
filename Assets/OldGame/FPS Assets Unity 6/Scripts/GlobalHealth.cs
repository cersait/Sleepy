using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalHealth : MonoBehaviour
{
    public static int currentHealth = 20; //Vi har en Health på 20
    public int internalHealth;

    void Update()
    {
        internalHealth = currentHealth;
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(0); //Om vi dör så laddas scenen om
            //Lägg till annan logik om du vill att något annat ska ske
        }
    }
}
