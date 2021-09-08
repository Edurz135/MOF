using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : Command
{
    public override void execute(Player player){
        player.Jump();
    }
}
