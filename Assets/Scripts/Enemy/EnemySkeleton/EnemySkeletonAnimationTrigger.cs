using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        //检测攻击范围内的碰撞体
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in collider2Ds)
        {
            //如果敌人被攻击，则受到伤害
            if (hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damage();
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
