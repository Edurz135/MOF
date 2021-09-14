﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public LayerMask whatIsTarget;
    public LayerMask collisionLayers;
    public bool stayOnCollisionLayer;
    public float damage;
    public float targetKnockback;

    void Start()
    {
        Invoke("DestroyBullet", lifetime);
    }

    public void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if((whatIsTarget.value & (1 << col.gameObject.layer)) > 0){
            float angle = transform.rotation.eulerAngles.z + 180;
            if(CanReceiveKnockback(col.gameObject)){
                AddKnockback(col.gameObject, angle, targetKnockback);
            }
            if(CanBeHit(col.gameObject)){
                Hit(col.gameObject, angle);
            }
            DestroyBullet();
        }

        // if(stayOnCollisionLayer){
        if((collisionLayers.value & (1 << col.gameObject.layer)) > 0){
            //Rigidbody2D rb = GetComponent<Rigidbody2D>();
            //rb.velocity = new Vector2(0, 0);
            //rb.isKinematic = true;
            DestroyBullet();
        }    
        // }
    }

    private bool CanReceiveKnockback(GameObject go){
        if(go.GetComponent<IKnockback>() != null){
            return true;
        }
        return false;
    }

    private void AddKnockback(GameObject go, float angle, float force){
        go.GetComponent<IKnockback>().Knockback(angle, force);
    }

    private bool CanBeHit(GameObject go){
        if(go.GetComponent<IHittable>() != null){
            return true;
        }
        return false;
    }

    private void Hit(GameObject go, float angle){
        go.GetComponent<IHittable>().TakeHit(angle);
    }

}
