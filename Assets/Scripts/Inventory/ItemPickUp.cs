using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public bool hasPickedUp = false;
    private bool isFocused = false;
	private Collider2D col;
	private Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();

		if(hasPickedUp){
			PickUp();
		}else{
			Drop();
		}
	}

	public void PickUp(){
		hasPickedUp = true;
		col.enabled = false;
		rb.velocity = new Vector2(0, 0);
		rb.isKinematic  = true;
	}

	public void Drop(){
		hasPickedUp = false;
		col.enabled = true;
		rb.isKinematic  = false;
	}

}
