using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Inventory inventory;
    public List<GameObject> pickableItems = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Space pressed");
            if(pickableItems.Count == 0) return;
            inventory.PickUpItem(pickableItems[0]);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            inventory.GetNextItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        GameObject item = col.gameObject;
        ItemPickUp itemPickUp = item.GetComponent<ItemPickUp>();
        
        if(itemPickUp != null && itemPickUp.hasPickedUp == false){
            pickableItems.Add(item);
            itemPickUp.OnFocused();
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        GameObject item = col.gameObject;
        ItemPickUp itemPickUp = item.GetComponent<ItemPickUp>();
        
        if(itemPickUp != null){
            pickableItems.Remove(item);
            itemPickUp.OnDefocused();
        }
    }
    
}
