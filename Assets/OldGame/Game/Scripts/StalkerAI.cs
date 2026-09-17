using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Ref till Unitys AI funktioner

public class StalkerAI : MonoBehaviour
{
    public GameObject stalkerDes; //Ref till det objektet vi vill att Enemyn ska förfölja
    NavMeshAgent stalkerAgent; //Ref till vår NavMeshAgent komponent
    public GameObject stalkerEnemy;

    void Start()
    {
        stalkerAgent = GetComponent<NavMeshAgent>(); //Ref till vår NavMeshAgent komponent
    }
    void Update()
    {
            stalkerAgent.SetDestination(stalkerDes.transform.position); //Rör dig emot StalkerDes
            stalkerEnemy.GetComponent<Animator>().Play("Walk"); //Spela Walk Anim
    }
}
