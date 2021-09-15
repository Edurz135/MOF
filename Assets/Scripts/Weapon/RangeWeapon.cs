using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    public string weaponName;

    [Header("SHOTGUN")]
    public bool isShotgun;
    public float shotgunRange;
    public int bulletsPerTap;

    [Header("STATS")]
    public float shootForce;
    public float timeBetweenShoots;
    private bool canShoot = true;
    public float reloadTime;
    private bool isReloading = false;

    public int magazineSize;
    public int bulletsLeft;

    public float controllerKnockback = 300f;
    public float targetKnockback = 500f;

    [Header("COMPONENTS")]
    public GameObject projectile;
    public Transform firePoint;

    [Header("EFFECTS")]
    public GameObject shootEffect;

    public delegate void shootCallback(float angle, float force);
    public shootCallback OnShoot;

    private void Start() {
        bulletsLeft = magazineSize;    
    }

    public override void Use(){
        if(!canShoot) return;
        if(isReloading) return;
        if(!HaveEnoughBullets()){
            Reload();
            return;
        }

        if(isShotgun){
            ShotgunShoot();
        }else{
            AutomaticShoot();
        }
    }

    private void Reload(){
        isReloading = true;
        bulletsLeft = magazineSize;
        Invoke("SetIsReloadingFalse", reloadTime);
    }

    #region SHOOT_TYPES
    private void AutomaticShoot(){
        ShootBulletWithAngleRotation((firePoint.eulerAngles.z + 90) % 360);
        bulletsLeft--;
        Invoke("SetCanShootTrue", timeBetweenShoots);
        canShoot = false;
    }
    private void ShotgunShoot(){
        float[] bulletDirection = this.GetBulletDirections(this.shotgunRange, (firePoint.eulerAngles.z + 90) % 360 , this.bulletsPerTap);
        foreach (float angleDir in bulletDirection)
        {
            this.ShootBulletWithAngleRotation(angleDir);
            bulletsLeft--;
        }
        
        Invoke("SetCanShootTrue", timeBetweenShoots);
        canShoot = false;
    }
    #endregion

    private void ShootBulletWithAngleRotation(float angle){
        GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        bullet.GetComponent<Bullet>().targetKnockback = targetKnockback;
        //AudioManager.instance.Play("Shoot1");
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.right * shootForce, ForceMode2D.Impulse);
        ScreenShakeController.instance.StartShake(0.1f, 0.1f);
        
        Instantiate(shootEffect, transform.position, Quaternion.LookRotation(DegreesToVector2(angle)));
        
        // Delegate
        OnShoot(angle, controllerKnockback / (isShotgun ? bulletsPerTap : 1));
    }

    private float[] GetBulletDirections(float rangeOfShoot, float centerOfRange, int nBullets){
        float halfRange = rangeOfShoot / 2;
        float start = centerOfRange - halfRange;
        float stop = centerOfRange + halfRange;

        return LinearSpace(start, stop, nBullets);
    }

    /// <returns>Returns num evenly spaced samples, calculated over the interval [start, stop].</returns>
    /// <param name="start">The starting value of the sequence. </param>
    /// <param name="stop">The end value of the sequence. </param>
    /// <param name="num">Number of samples to generate. </param>
    private float[] LinearSpace(float start, float stop, int num){
        float interval = stop - start;
        float[] result = new float[num];
        float step = interval / (num - 1);

        for (int i = 0; i < num; i++) {
            result[i] = start + step * i;
        }
        return result;
    }

    private Vector2 DegreesToVector2(float angle){
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    private bool HaveEnoughBullets(){
        if(bulletsLeft <= 0){
            return false;
        }
        return true;
    }
    private void SetCanShootTrue(){
        canShoot = true;
    }
    private void SetIsReloadingFalse(){
        isReloading = false;
    }
}
