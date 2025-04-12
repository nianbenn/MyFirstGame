using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//瞄准投剑
public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, 0);
        player.skill.swordSkill.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if(Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        //鼠标的方位
        Vector2 mousePosiyion  = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosiyion.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();
        else if(mousePosiyion.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();
    }
}
