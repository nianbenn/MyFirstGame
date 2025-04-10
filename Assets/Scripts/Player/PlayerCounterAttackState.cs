using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//格挡反击状态
public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);//成功格挡为false
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //检测攻击范围内的碰撞体
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in collider2Ds)
        {
            //如果敌人被攻击，则受到伤害
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;//防止状态结束导致动画没有播放完
                    player.anim.SetBool("SuccessfulCounterAttack", true);//成功格挡为true
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)//状态时间结束，或者动画结束退出状态
            stateMachine.ChangeState(player.idleState);
    }
}
