using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderController : MonoBehaviour
{
    public RangeWeapon currentWeapon;
    public Vector2 currentDir;
    public float rotateSpeed;
    public GameObject pivot;
    
    public void Attack(){
        currentWeapon.Use();
    }

    public void AimToTarget(Transform target){
        Vector2 direction = (target.position - transform.position).normalized;
        currentDir = Vector2.Lerp(currentDir, direction, rotateSpeed * Time.deltaTime);
        SetRotation(currentDir);
    }

    public void AimToDirection(Vector2 dir){
        Vector2 direction = dir.normalized;
        currentDir = Vector2.Lerp(currentDir, direction, rotateSpeed * Time.deltaTime);
        SetRotation(currentDir);
    }

    public void SetRotation(Vector2 dir){
        if(0 < dir.x){
            pivot.transform.right = dir;
            FaceToDirection(1);
        }else{
            pivot.transform.right = -dir;
            FaceToDirection(-1);
        }
    }
    
    public void FaceToDirection(int dir){
        Vector3 newScale = pivot.transform.localScale;
        newScale.x = dir;
        pivot.transform.localScale = newScale;
    }
}
