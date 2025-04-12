using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制克隆体的脚本
public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private Transform closestEnemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - (Time.deltaTime * colorLosingSpeed));
            if (spriteRenderer.color.a < 0)
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform newTransform, float cloneDuration,bool canAttack)
    {
        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        transform.position = newTransform.position;
        cloneTimer = cloneDuration;

        FaceClosestTarget();
    }

    private void AnimationTrigger() // 检测动画是否完成
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        //检测攻击范围内的碰撞体
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in collider2Ds)
        {
            //如果敌人被攻击，则受到伤害
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);//找到半径在25范围内的所有碰撞体
        float closestDistance = Mathf.Infinity;

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) 
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.GetComponent<Enemy>().transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
