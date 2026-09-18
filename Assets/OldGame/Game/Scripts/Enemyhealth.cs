using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyhealth : MonoBehaviour
{
    public float health = 50f;
    public GameObject theEnemy;

    public void TakeDamage(float damageamount)
    {
        health -= damageamount;
        if (health <= 0f)
        {
            Died();
        }
    }
    void Died()
    {
        theEnemy.GetComponent<Animator>().Play("Die"); //Namnet på din animation
        this.GetComponent<StalkerAI>().enabled = false; //Vår Enemy kommer att dö
        //this.GetComponent<Enemyattack>().enabled = false; //Vår Enemy kommer att dö
        this.GetComponent<BoxCollider>().enabled = false; //Vår Enemys collider kommer att försvinna

        //Här kan du lägga till med logik om du vill att fiendens kropp även ska förvinna efter ett tag
    }
}