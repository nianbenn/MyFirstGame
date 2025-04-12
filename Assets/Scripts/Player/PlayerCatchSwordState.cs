using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//»ØÊÕ½£
public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        if (sword.position.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();
        else if (sword.position.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
