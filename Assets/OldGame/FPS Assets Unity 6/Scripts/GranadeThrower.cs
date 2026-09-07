using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePre;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowGrenade();
        }
    }
    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePre, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
