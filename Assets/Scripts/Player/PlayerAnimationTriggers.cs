using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger() // ��⶯���Ƿ����
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        //��⹥����Χ�ڵ���ײ��
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in collider2Ds)
        {
            //������˱����������ܵ��˺�
            if(hit.GetComponent<Enemy>()!=null)
                hit.GetComponent<Enemy>().Damage();
        }
    }
}
