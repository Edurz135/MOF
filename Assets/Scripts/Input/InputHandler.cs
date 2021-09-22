using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //public Command jumpButton;
    //public Command horizontalMoveButton;
    public JoystickCommand joystickCommand;
    public PickUpCommand pickUpCommand;
    public JumpCommand jumpCommand;
    public Joystick joystick;
    public PlayerController player;
    public KeyCode pickUpButton;
    public KeyCode jumpButton;

    //public bool zeroPlayer = true;
    //private Vector2 dir;
    
    void HandleInput(){
        //if(Input.GetAxis("Horizontal") != 0) horizontalMoveButton.execute(player);
        if(Input.GetKeyDown(pickUpButton)) pickUpCommand.execute(player);
        if(Input.GetKeyDown(jumpButton)) jumpCommand.execute(player);
        if(joystick.Direction.magnitude > 0.1f) joystickCommand.execute(player, joystick);
        // if(dir.magnitude > 0.1f) joystickCommand.execute(player, dir);
    }

    void Update() {
        HandleInput();
        //Inpu();
    }

    // void Inpu(){
    //     if(zeroPlayer){
    //         if (Input.GetKey(KeyCode.A))
    //             dir.x -= 0.1f;
    //         if (Input.GetKey(KeyCode.D))
    //             dir.x += 0.1f;
    //         if (Input.GetKey(KeyCode.W))
    //             dir.y += 0.1f;
    //         if (Input.GetKey(KeyCode.S))
    //             dir.y -= 0.1f;
    //     }else{
    //         if (Input.GetKey(KeyCode.LeftArrow))
    //             dir.x -= 0.1f;
    //         if (Input.GetKey(KeyCode.RightArrow))
    //             dir.x += 0.1f;
    //         if (Input.GetKey(KeyCode.UpArrow))
    //             dir.y += 0.1f;
    //         if (Input.GetKey(KeyCode.DownArrow))
    //             dir.y -= 0.1f;

    //     }
    //     dir = dir.normalized;
    // }
}
