using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCommand : Command
{
    public override void execute(Player player){
        player.PickUp();
    }
}
