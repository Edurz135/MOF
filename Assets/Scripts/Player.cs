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

    [Header("PARTICLES")]
    public GameObject hitEffect;

    private void Awake() {
        RangeWeapon rw = (RangeWeapon) weaponHolder.currentWeapon;
        rw.OnShoot += Knockback;
    }

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z")){ // borrar
            timeM.DoSlowmotion();
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            if(pickUpController.AreNearbyItems()){
                Debug.Log("PICK");
                PickUpItem();
                Debug.Log("setting");
                weaponHolder.currentWeapon = inventory.GetCurrentItem().GetComponent<Weapon>();
                Debug.Log("finish");
            } else {
                Debug.Log("CHANGE");
                SetItemOnWeaponHolder();
            }
        }
    }

    public void Jump(){
        //rb.AddForce(new Vector2(0, jumpForce));
    }

    public void PickUpItem(){
        pickUpController.PickUp();
        //configurar inputs para inventario.
    }

    public void SetItemOnWeaponHolder(){
        weaponHolder.currentWeapon = inventory.GetNextItem().GetComponent<Weapon>();
    }

    public void Aim(Joystick joystick){
        weaponHolder.Attack();
        weaponHolder.AimToDirection(joystick.Direction);
        //Debug.Log(joystick.Horizontal);
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
    }

    private Vector2 DegreesToVector2(float angle){
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    
}
