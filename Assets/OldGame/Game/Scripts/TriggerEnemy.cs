using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemy : MonoBehaviour
{
    public GameObject theEnemy;
    void OnTriggerEnter()
    {
        GetComponent<BoxCollider>().enabled = false;
        theEnemy.SetActive(true);
    }
}
