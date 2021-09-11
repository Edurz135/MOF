﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKnockback
{
    [Header("COMPONENTS")]
    public Rigidbody2D rb;
    public WeaponHolderController weapon;

    [Header("STATS")]
    float jumpForce = 100;

    // Start is called before the first frame update
    private void Awake() {
        RangeWeapon rw = (RangeWeapon) weapon.currentWeapon;
        rw.OnShoot += Knockback;
    }

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump(){
        rb.AddForce(new Vector2(0, jumpForce));
    }

    public void Aim(Joystick joystick){
        weapon.Attack();
        weapon.AimToDirection(joystick.Direction);
        //Debug.Log(joystick.Horizontal);
        //rb.AddForce(new Vector2(0, jumpForce));
    }

    public void Knockback(float angle, float force){
        rb.AddForce(-DegreesToVector2(angle) * force);
    }

    private Vector2 DegreesToVector2(float angle){
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }
}
