using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BturretLogic;

public class BTAmmoLogic : MonoBehavior
{
    public float damage;
    public float ExplosionRadius = 2f;

    void Start()
    {
        Bturret
    }

    public void OnCollisionEnter(Collider other)
    {
        if (other.CompareTag("RoadTile"))
        {
            Debug.Log("Ammo collided with RoadTile. Initialising explosion.");
            BTAmmoExplode();
        }
    }

    public void BTAmmoExplode()
    {

    }
}
