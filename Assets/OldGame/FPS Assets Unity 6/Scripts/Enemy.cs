using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f;
    //public GameObject theEnemy;
    public void TakeDamage(float damageamount)
    {
        health -= damageamount;
        if(health <= 0f)
        {
            Died();
        }
    }
    void Died()
    {
        Destroy(gameObject);
        /*theEnemy.GetComponent<Animation>().Stop("Walk"); //Namnet på din animation
        theEnemy.GetComponent<Animation>().Play("Die"); //Namnet på din animation
        this.GetComponent<EnemyAI>().enabled = false; //Vår Enemy kommer att dö
        this.GetComponent<BoxCollider>().enabled = false; //Vår Enemys collider kommer att försvinna*/
    }
}
