using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickCommand : Command
{
    public override void execute(Player player){
        //player.Jump();
    }
    public void execute(Player player, Joystick joystick){
        player.Aim(joystick);
    }
}
