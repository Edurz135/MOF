using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 0)]
public class Item : ScriptableObject {
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
    public Sprite body = null; 

    [Header("EFFECTS")]
    public GameObject shootEffect;

}
