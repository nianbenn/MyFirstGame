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

    private float freezeTimeDuration;//����ʱ��
    private float returnSpeed;

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Bounce info")]
    private float bouseSpeed;//�����ٶ�
    private bool isBouncing;//����
    private int bounceAmount;//����������
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;//�����ȴʱ��

    //private float spinDirection;//�����Ľ��ƶ�����

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

        Invoke("DestroyMe", 7); // 7������ٽ�

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
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // ������������Լ��
        //rb.isKinematic = false;// �ر��˶�ѧģʽ
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate) // �������ǰ�����ٶȷ������
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
        //���뷴���߼�
        BounceLogic();

        //���������߼�
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
                    //��ȴʱ���ѹ�����ʼ�Ե�������˺�
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
        wasStopped = true;//ֹͣ��ת
        rb.constraints = RigidbodyConstraints2D.FreezePosition; // ����λ��
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
                bounceAmount--;//����������һ
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;//���ս�
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)//�ս���ʱ�򲻻��˺�������
            return;

        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision); // ����ײ�巴��

        StuckInto(collision);//Ƕ��
    }

    //�����ܵ��˺�
    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);//��ͣ����freezeTimeDurationʱ��
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

    //Ƕ��
    private void StuckInto(Collider2D collision)
    {
        //��͸��Ƕ��
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        //����ʱ��Ƕ��
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }   

        canRotate = false;
        cd.enabled = false; // ������ײ�壬�����δ���

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // ������������Լ��

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;// ��Ϊ��ײ������Ӷ��󣬼�������ײ�����ƶ�
    }
}
