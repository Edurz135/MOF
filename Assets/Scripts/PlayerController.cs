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
    public ScoreList scoreList;
    public HealthBar healthBar;
    public int playerID;
    [Space]
    public TMP_Text healthTxt;
    public TMP_Text ammoTxt;
    public TMP_Text pingTxt;
    public TMP_Text nameTxt;
    [Space]
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;
    public float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;

    [Header("STATS")]
    public float baseHealth = 100f;
    public float currentHealth;
    public float jumpForce = 500f;
    public float moveSpeed = 10f;
    private bool beingknocked = false;
    [HideInInspector] public Vector2 desiredVelocity;
    [HideInInspector] public Vector2 currentVelocity;
    [HideInInspector] public Vector2 impulseVelocity;

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
        UpdateUI();
        
        if(!PV.IsMine) {
            Destroy(rb);
            Destroy(canvas);
            Destroy(camera.gameObject);
        }
    }

    public void Name(int name) {
        PV.RPC("RPC_Name", RpcTarget.All, name);
    }

    [PunRPC]
    void RPC_Name(int name) {
        nameTxt.text = name.ToString();
    }


    // Update is called once per frame
    Vector2 shootDirection;
    void Update()
    {
        if(PV.IsMine) {
            // if (Input.GetKeyDown("z")) { // borrar
            //     //timeM.DoSlowmotion();
            //     AudioManager.instance.Play("die1");
            //     // ScoreManager.instance.AddPoint(playerID);
            // }
            // if (Input.GetKeyDown("x")) { // borrar
            //     AudioManager.instance.Play("hit1");
            //     //timeM.DoSlowmotion();
                
            //     // ScoreManager.instance.AddPoint(playerID);
            // }
            
            if(Input.GetKeyDown(KeyCode.LeftShift)) {
                PickUp();
            }

            if (Input.GetMouseButtonDown(0)) {
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

    private void FixedUpdate() {
        if(PV.IsMine) {
            Movement();
            Jump();
        }
    }

    public void Movement() {
        if(beingknocked) return;
        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
        }
    }

    public void Jump() {
        //if(beingknocked) return;
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if(isGrounded == true && Input.GetKeyDown("w")) {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if(Input.GetKey("w") && isJumping == true) {
            if(jumpTimeCounter > 0) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }else{
                isJumping = false;
            }
        }
        if(Input.GetKeyUp("w")) {
            isJumping = false;
        }
    }

    public void PickUp() {
        // if(pickUpController.AreNearbyItems()) {
        //     pickUpController.PickUp();
        // } else {
        // }
        inventory.NextItem();
    }

    public void Attack() {
        weaponHolder.Attack();
    }

    public void Aim(Vector2 direction) {
        weaponHolder.AimToDirection(direction);
    }

    public void Knockback(float angle, float force) {
        PV.RPC("RPC_Knockback", RpcTarget.All, angle, force);
        // Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-dir));
    }

    public void TakeHit(float angle) {
        PV.RPC("RPC_TakeHit", RpcTarget.All, angle);
    }

    public void TakeDamage(float amount, int id) {
		PV.RPC("RPC_TakeDamage", RpcTarget.All, amount, id);
    }

    [PunRPC]
    void RPC_Knockback(float angle, float force) {
        if(!PV.IsMine)
			return;
            
        if(rb == null) {
            Debug.Log("RB is null");
            return;
        }
        // impulseVelocity += dir * force / 10;
        Vector2 dir = -DegreesToVector2(angle);
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        beingknocked = true;
        Invoke("SetBeingKnockedFalse", 0.35f);
    }

    [PunRPC]
    void RPC_TakeHit(float angle) {
        // if(!PV.IsMine)
		// 	return;
        
        Vector2 dir = DegreesToVector2(angle); 
        Instantiate(hitEffect, transform.position, Quaternion.LookRotation(-dir));
        ScreenShakeController.instance.StartShake(0.2f, 0.2f);
    }

	[PunRPC]
	void RPC_TakeDamage(float amount, int killerId) {
		if(!PV.IsMine)
			return;

        AudioManager.instance.Play("hit1");
		currentHealth -= amount;
        healthBar.UpdateHealthBar(currentHealth / baseHealth);
		if(currentHealth <= 0) {
            ScoreManager.instance.AddPoint(killerId);
            AudioManager.instance.Play("die1");
            this.Die();
        }
	}

    void Die() {
		playerManager.Die();
	}

    void UpdateUI() {
        healthTxt.text = "HEALTH: " + currentHealth;
        ammoTxt.text = weaponHolder.currentWeapon.bulletsLeft.ToString();
        pingTxt.text = "PING: " + PhotonNetwork.GetPing().ToString();
    }

    private Vector2 DegreesToVector2(float angle) {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.IsWriting) {
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

    void SetBeingKnockedFalse(){
        this.beingknocked = false;
    }
    
}
