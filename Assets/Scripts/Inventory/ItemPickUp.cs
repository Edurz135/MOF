using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public bool hasPickedUp = false;
    private bool isFocused = false;

    public void OnFocused ()
	{
		isFocused = true;
	}

	// Called when the object is no longer focused
	public void OnDefocused ()
	{
		isFocused = false;
	}
}
