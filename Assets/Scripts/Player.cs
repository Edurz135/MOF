using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("COMPONENTS")]
    public Rigidbody2D rb;

    [Header("STATS")]
    float jumpForce = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump(){
        rb.AddForce(new Vector2(0, jumpForce));
    }


}
