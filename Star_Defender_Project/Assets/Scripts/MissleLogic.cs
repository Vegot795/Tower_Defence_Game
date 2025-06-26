using UnityEngine;

public class MissleLogic : MonoBehaviour 
{
    private GameObject target;
    private float damage;
    private Rigidbody rb;

    public void SetTarget(GameObject targetEnemy)
    {
        this.target = targetEnemy;
    }

    public void SetDamage(float turretDamage)
    {
        this.damage = turretDamage;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            DealDamage(other.gameObject);
            Destroy(gameObject);
        }
    }



    public void DealDamage(GameObject target)
    {
        //Debug.Log("Attempting to deal damage.");
        var hittable = target.GetComponent<IDamager>();
        if (hittable != null)
        {
            //Debug.Log($"Target has IDamager. Dealing {damage} damage to {target.name}.");
            hittable.ReceiveDamage((int)damage);
        }
        else
        {
            Debug.Log("Target does not implement IDamager.");
        }
    }

}
