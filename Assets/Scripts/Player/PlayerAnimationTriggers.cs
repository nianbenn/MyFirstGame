using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger() // 检测动画是否完成
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        //检测攻击范围内的碰撞体
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in collider2Ds)
        {
            //如果敌人被攻击，则受到伤害
            if(hit.GetComponent<Enemy>()!=null)
                hit.GetComponent<Enemy>().Damage();
        }
    }
}
