using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //public Command jumpButton;
    //public Command horizontalMoveButton;
    public JoystickCommand joystickCommand;
    public PickUpCommand pickUpCommand;
    public Joystick joystick;
    public Player player;
    public KeyCode pickUpButton;
    
    void HandleInput(){
        //if(Input.GetAxis("Horizontal") != 0) horizontalMoveButton.execute(player);
        if(Input.GetKeyDown(pickUpButton)) pickUpCommand.execute(player);
        if(joystick.Direction.magnitude > 0.1f) joystickCommand.execute(player, joystick);
    }

    void Update() {
        HandleInput();
    }
}
