using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Command jumpButton;
    public Command horizontalMoveButton;
    public JoystickCommand joystickCommand;
    public Joystick joystick;
    public Player player;
    
    void HandleInput(){
        if(Input.GetAxis("Horizontal") != 0) horizontalMoveButton.execute(player);
        if(Input.GetKeyDown(KeyCode.Space)) jumpButton.execute(player);
        if(joystick.Direction.magnitude > 0.1f) joystickCommand.execute(player, joystick);
    }

    void Update() {
        HandleInput();
    }
}
