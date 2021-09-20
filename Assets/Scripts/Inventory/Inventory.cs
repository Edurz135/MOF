using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public int space = 3;	// Amount of item spaces
    public int currentItemIndex = 0;

	// Our current list of items in the inventory
	public List<GameObject> items = new List<GameObject>();
    public int nItems;

    public Transform handTransform;

    public GameObject GetCurrentItem(){
        return items[currentItemIndex];
    }

    public GameObject GetNextItem(){
        if(nItems <= 1) return null;
        Debug.Log("ämtes " + currentItemIndex);
        currentItemIndex = (currentItemIndex + 1) % nItems;
        Debug.Log("despues" + currentItemIndex);
        
        items[currentItemIndex].SetActive(true);
        UnusedItemsSetActiveFalse();
        return items[currentItemIndex];
    }

    private void UnusedItemsSetActiveFalse(){
        for(int i = 0; i < nItems; i++){
            if(i != currentItemIndex){
                items[i].SetActive(false);
            }
        }
    }

    public void PickUpItem(GameObject item){
        ItemPickUp itemPick = item.GetComponent<ItemPickUp>();
        if(itemPick.hasPickedUp) return;
        
        if(AreFreeSlots()){
            Add(item);
        }else{
            ChangeItem(item);
        }
        itemPick.PickUp();
    }


    public bool AreFreeSlots(){
        if (nItems >= space) {
            Debug.Log ("Not enough space.");
            return false;
        }
        return true;
    }
	
    // Add a new item if enough room
	private void Add (GameObject item)
	{
        nItems ++;
        items.Add(item);
        item.transform.position = handTransform.position;
        item.transform.rotation = handTransform.rotation;
        item.transform.parent = handTransform;
        GetNextItem();
	}

    private void ChangeItem(GameObject item){
        Drop(items[currentItemIndex]);
        Add(item);
        UnusedItemsSetActiveFalse();
    }

    private void Drop(GameObject item){
		items.Remove(item);
        item.GetComponent<ItemPickUp>().Drop();
        item.transform.position = transform.position;
        item.transform.parent = null; 
    }
}
