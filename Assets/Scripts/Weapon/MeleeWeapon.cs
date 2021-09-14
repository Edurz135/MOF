using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public string weaponName;

    [Header("STATS")]
    public int damage;
    public float attackCooldown;
    public LayerMask whatIsTarget;
    public bool canAttack = true;

    public bool isAttacking = false;

    [Header("COMPONENTS")]
    public Animator animator;

    public override void Use(){
        if(!canAttack) return;
        isAttacking = true;
        animator.SetTrigger("Attack");
        canAttack = false;
        Invoke("SetCanAttackTrue", attackCooldown);
    }

    private void SetCanAttackTrue(){
        canAttack = true;
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if(!isAttacking) return;
        if((whatIsTarget.value & (1 << col.gameObject.layer)) > 0){
            IDamageable damageObj = col.GetComponent<IDamageable>();
            if(damageObj != null){
                damageObj.TakeDamage(damage);
                //damageObj.AddStun(DegreeToVector2(transform.eulerAngles.z));
            }
        }
        isAttacking = false;
    }

    public static Vector2 DegreeToVector2(float degree) {
        float radian = degree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
