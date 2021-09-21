using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKnockback, IHittable
{
    [Header("COMPONENTS")]
    public Rigidbody2D rb;
    public WeaponHolderController weaponHolder;
    public TimeManager timeM; //borrar
    public Inventory inventory;
    public PickUpController pickUpController;

    [Header("STATS")]
    public float baseHealth = 100f;
    public float currentHealth;
    public float jumpForce = 500f;

    [Header("PARTICLES")]
    public GameObject hitEffect;

    void Start()
    {
        currentHealth = baseHealth;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z")){ // borrar
            timeM.DoSlowmotion();
        }

    }

    public void Jump(){
        rb.AddForce(new Vector2(0, jumpForce));
    }

    public void PickUp(){
        if(pickUpController.AreNearbyItems()){
            pickUpController.PickUp();
        } else {
            inventory.NextItem();
        }
    }

    public void Aim(Vector2 direction){
        weaponHolder.Attack();
        weaponHolder.AimToDirection(direction);
        //rb.AddForce(new Vector2(0, jumpForce));
    }

    public void Knockback(float angle, float force){
        Vector2 dir = -DegreesToVector2(angle); 
        rb.AddForce(dir * force);
        // Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-dir));
    }

    public void TakeHit(float angle){
        Vector2 dir = DegreesToVector2(angle); 
        Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-dir));
        ScreenShakeController.instance.StartShake(0.2f, 0.2f);
        currentHealth -= 10f;
    }

    private Vector2 DegreesToVector2(float angle){
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    
}
