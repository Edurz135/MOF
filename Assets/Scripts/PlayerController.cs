using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour, IKnockback, IHittable, IDamageable, IPunObservable
{
    [Header("COMPONENTS")]
    public Rigidbody2D rb;
    public WeaponHolderController weaponHolder;
    public TimeManager timeM; //borrar
    public Inventory inventory;
    public PickUpController pickUpController;
    public Camera camera;
    public GameObject canvas;
    private PlayerManager playerManager;
    [Space]
    public TMP_Text healthTxt;

    [Header("STATS")]
    public float baseHealth = 100f;
    public float currentHealth;
    public float jumpForce = 500f;

    [Header("PARTICLES")]
    public GameObject hitEffect;

    private PhotonView PV;
    private Vector2 weaponDir;

    private void Awake() {
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        AddObservable();
        currentHealth = baseHealth;
        if(!PV.IsMine){
            Destroy(rb);
            Destroy(canvas);
            Destroy(camera.gameObject);
        }
    }


    // Update is called once per frame
    Vector2 shootDirection;
    void Update()
    {
        if(PV.IsMine){
            // if (Input.GetKeyDown("z")){ // borrar
            //     timeM.DoSlowmotion();
            // }
            
            if(Input.GetKeyDown("space")){
                TakeDamage(10f);
            }

            if (Input.GetMouseButtonDown(0)){
                Attack();
            }

            shootDirection = Input.mousePosition;
            shootDirection = camera.ScreenToWorldPoint(shootDirection);
            shootDirection = shootDirection - (Vector2) transform.position;
            Aim(shootDirection);

            UpdateUI();
        }else{
            Aim(weaponDir);
        }
    }

    public void Jump(){
        rb.AddForce(new Vector2(0, jumpForce));
    }

    public void PickUp(){
        // if(pickUpController.AreNearbyItems()){
        //     pickUpController.PickUp();
        // } else {
        // }
        inventory.NextItem();
    }

    public void Attack(){
        weaponHolder.Attack();
    }

    public void Aim(Vector2 direction){
        weaponHolder.AimToDirection(direction);
    }

    public void Knockback(float angle, float force){
        if(rb == null){
            Debug.Log("RB is null");
            return;
        }
        Vector2 dir = -DegreesToVector2(angle); 
        rb.AddForce(dir * force);
        // Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-dir));
    }

    public void TakeHit(float angle){
        Vector2 dir = DegreesToVector2(angle); 
        Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-dir));
        ScreenShakeController.instance.StartShake(0.2f, 0.2f);
    }

    public void TakeDamage(float amount){
		PV.RPC("RPC_TakeDamage", RpcTarget.All, amount);
    }

	[PunRPC]
	void RPC_TakeDamage(float amount){
		if(!PV.IsMine)
			return;

		currentHealth -= amount;

		if(currentHealth <= 0){
            this.Die();
        }
	}

    void Die(){
		playerManager.Die();
	}

    void UpdateUI(){
        healthTxt.text = "HEALTH: " + currentHealth;
    }

    private Vector2 DegreesToVector2(float angle){
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(weaponHolder.currentDir);
            stream.SendNext(currentHealth);
        }else{
            weaponDir = (Vector2) stream.ReceiveNext();
            currentHealth = (float) stream.ReceiveNext();
        }
    }

    private void AddObservable()
    {
        if (!PV.ObservedComponents.Contains(this))
        {
            PV.ObservedComponents.Add(this);
        }
    }
    
}
