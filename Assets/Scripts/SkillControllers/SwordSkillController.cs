using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;//冻结时间
    private float returnSpeed;

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Bounce info")]
    private float bouseSpeed;//反弹速度
    private bool isBouncing;//反弹
    private int bounceAmount;//反弹的数量
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;//打击冷却时间

    //private float spinDirection;//自旋的剑移动方向

    //private float test = 0;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)
    {
        rb.velocity = dir;
        rb.gravityScale = gravityScale;
        this.player = player;
        this.freezeTimeDuration = freezeTimeDuration;
        this.returnSpeed = returnSpeed;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        Invoke("DestroyMe", 7); // 7秒后销毁剑

        //spinDirection = Mathf.Clamp(rb.velocity.x, 1, -1);
    }

    public void SetupBounce(bool isBouncing, int amountOfBounces, float bounceSpeed)
    {
        this.isBouncing = isBouncing;
        this.bounceAmount = amountOfBounces;
        this.bouseSpeed = bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有物理约束
        //rb.isKinematic = false;// 关闭运动学模式
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate) // 将物体的前方与速度方向对齐
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.ClearTheSword();
                isReturning = false;
            }
        }
        //进入反弹逻辑
        BounceLogic();

        //进入自旋逻辑
        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            //test += Time.deltaTime;
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer <= 0)
                {
                    isReturning = true;
                    //Debug.Log(test);
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    //冷却时间已过，开始对敌人造成伤害
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }

                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;//停止旋转
        rb.constraints = RigidbodyConstraints2D.FreezePosition; // 冻结位置
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bouseSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;//反弹次数减一
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;//回收剑
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)//收剑的时候不会伤害到敌人
            return;

        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision); // 对碰撞体反弹

        StuckInto(collision);//嵌入
    }

    //剑技能的伤害
    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);//暂停敌人freezeTimeDuration时间
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    //嵌入
    private void StuckInto(Collider2D collision)
    {
        //穿透则不嵌入
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        //自旋时不嵌入
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }   

        canRotate = false;
        cd.enabled = false; // 禁用碰撞体，避免多次触发

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有物理约束

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;// 设为碰撞物体的子对象，即跟着碰撞物体移动
    }
}
