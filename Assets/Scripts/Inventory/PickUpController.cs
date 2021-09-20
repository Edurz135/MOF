using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Inventory inventory;
    public List<GameObject> pickableItems = new List<GameObject>();

    public bool AreNearbyItems(){
        if(pickableItems.Count == 0){
            return false;
        }
        return true;
    }

    public void PickUp(){
        if(AreNearbyItems()){
            inventory.PickUpItem(pickableItems[0]);
            pickableItems.Remove(pickableItems[0]);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        GameObject item = col.gameObject;
        if(item.GetComponent<ItemPickUp>() != null){
            pickableItems.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        GameObject item = col.gameObject;
        if(item.GetComponent<ItemPickUp>() != null){
            pickableItems.Remove(item);
        }
    }
    
}
