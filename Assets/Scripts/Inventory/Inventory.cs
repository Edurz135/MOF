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

    public GameObject GetNextItem(){
        if(nItems <= 1) return items[0];
        currentItemIndex = (currentItemIndex + 1) % nItems;
        
        items[currentItemIndex].SetActive(true);
        UnusedItemsSetActiveFalse();
        return items[currentItemIndex];
    }

    private void UnusedItemsSetActiveFalse(){
        for(int i = 0; i < nItems; i++ ){
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

        itemPick.hasPickedUp = true;
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
        item.transform.position = transform.position;
        item.transform.parent = transform; 
	}

    private void ChangeItem(GameObject item){
        Drop(items[currentItemIndex]);
        Add(item);
    }

    private void Drop(GameObject item){
		items.Remove(item);
        item.GetComponent<ItemPickUp>().hasPickedUp = false;
        item.transform.position = transform.position;
        item.transform.parent = null; 
    }

	// Remove an item
	// public void Remove (GameObject item)
	// {
    //     nItems --;
	// 	items.Remove(item);
    //     item.GetComponent<ItemPickUp>().hasPickedUp = false;
    //     item.transform.position = transform.position;
    //     item.transform.parent = null; 
	// }
}
