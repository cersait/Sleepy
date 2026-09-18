using UnityEngine;
public class Gun2 : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera cam;
    public Animator anim;
    public ParticleSystem muzzelflash;
    private float fireRate = 0.2f;
    private float nextFire = 0f;
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
            anim.Play("FirePistol3");
        } else if (Input.GetButtonUp("Fire1"))
       {
           anim.Play("New State");
       }
    }
  
    void Shoot()
    {
       muzzelflash.Play();
       RaycastHit hit;
       if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Enemyhealth enemy = hit.transform.GetComponent<Enemyhealth>();
            if(enemy != null) //kalla bara om vi träffar enemy, inte om vi träffar andra komponenter
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
