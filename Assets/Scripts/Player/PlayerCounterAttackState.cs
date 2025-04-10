using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//�񵲷���״̬
public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);//�ɹ���Ϊfalse
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //��⹥����Χ�ڵ���ײ��
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in collider2Ds)
        {
            //������˱����������ܵ��˺�
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;//��ֹ״̬�������¶���û�в�����
                    player.anim.SetBool("SuccessfulCounterAttack", true);//�ɹ���Ϊtrue
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)//״̬ʱ����������߶��������˳�״̬
            stateMachine.ChangeState(player.idleState);
    }
}
