using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMoveCommand : Command
{
    public override void execute(Player player){
        player.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 10, player.rb.velocity.y);  
    }
}
