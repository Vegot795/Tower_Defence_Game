using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BTAmmoLogic : MonoBehaviour
{
    public Transform target;
    public float damage;
    public float ExplosionRadius = 10f;
    public float speed = 20f;

    private void Start()
    {
        

    }
    public void SetDamage(float turretDamage)
    {
        this.damage = turretDamage;
        Debug.Log($"Passed Damage Value {damage}");
    }

    public void SetTarget(Transform targetEnemy)
    {
        this.target = targetEnemy;
        Debug.Log($"Target set to {target.name}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MAP"))
        {
            //Debug.Log("Ammo collided with RoadTile layer. Initialising explosion.");
            BTAmmoExplode();
            Destroy(gameObject);

        }
    }

    private void BTAmmoExplode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        //Debug.Log($"Objects in range of explosion: {hitColliders.Length}");

        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                var hittable = hit.GetComponent<IDamager>();
                //Debug.Log($"Target has IDamager. Dealing {damage} damage to {hit.gameObject.name}.");
                if (hittable != null)
                {
                    //Debug.Log("Object hit by explosion and implements IDamager: " + hit.name);
                    hittable.ReceiveDamage(damage);
                }
                else
                {
                    continue;
                }
            }
            else
            {
                // Skip objects that do not have the "Enemy" tag
                continue;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (this == null) return; // Prevents drawing if object is being destroyed
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

}
