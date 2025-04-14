using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

///黑洞技能热键控制器
///出现黑洞后，怪物头顶出现A
public class BlackholeHotKeyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackhole;

    public void SetupHotKey(KeyCode myHotKey,Transform myEnemy,BlackholeSkillController myBlackhole)
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.myText = GetComponentInChildren<TextMeshProUGUI>();

        this.myHotKey = myHotKey;
        this.myText.text = myHotKey.ToString();
        this.myEnemy = myEnemy;
        this.blackhole = myBlackhole;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackhole.AddEnemyToList(myEnemy);

            //按下之后，隐藏热键
            myText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}
