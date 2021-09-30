using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public int space = 3;	// Amount of item spaces
    public int currentItemIndex = 0;
    public PlayerController owner;
	// Our current list of items in the inventory
	public List<GameObject> items = new List<GameObject>();
    public int nItems;

    public WeaponHolderController weaponHolderController;
    public Transform handTransform;

    public void PickUpItem(GameObject item){
        ItemPickUp itemPick = item.GetComponent<ItemPickUp>();
        if(itemPick.hasPickedUp) return;
        
        if(AreFreeSlots()){
            nItems ++;
            Add(item);
        }else{
            ChangeItem(item);
        }
        itemPick.PickUp();
    }

    public void NextItem(){
        int newIdx = (currentItemIndex + 1) % nItems;
        SetWeaponWithIndex(newIdx);
    }

    private void UnusedItemsSetActiveFalse(){
        for(int i = 0; i < nItems; i++){
            if(i != currentItemIndex){
                items[i].SetActive(false);
            }
        }
    }

    public bool AreFreeSlots(){
        if (nItems < space) {
            return true;
        }
        return false;
    }
	
    // Add a new item if enough room
	private void Add (GameObject item)
	{
        items.Add(item);
        item.transform.position = handTransform.position;
        item.transform.rotation = handTransform.rotation;
        item.transform.parent = handTransform;
        item.transform.localScale = new Vector3(1, 1, 1);
        UnusedItemsSetActiveFalse();
        SetWeaponWithIndex(nItems - 1);
        item.GetComponent<Weapon>().owner = owner;
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

    private void SetWeaponWithIndex(int idx){
        if(idx >= nItems) return;

        currentItemIndex = idx;
        UnusedItemsSetActiveFalse();
        items[currentItemIndex].SetActive(true);
        weaponHolderController.currentWeapon = items[currentItemIndex].GetComponent<RangeWeapon>();
    }
}
