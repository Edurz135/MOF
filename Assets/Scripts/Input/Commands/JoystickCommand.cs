using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickCommand : Command
{
    public override void execute(PlayerController player){
        //player.Jump();
    }
    public void execute(PlayerController player, Joystick joystick){
        player.Aim(joystick.Direction);
    }
    public void execute(PlayerController player, Vector2 dir){
        player.Aim(dir);
    }
}
