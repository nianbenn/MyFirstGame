using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

///�ڶ������ȼ�������
///���ֺڶ��󣬹���ͷ������A
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

            //����֮�������ȼ�
            myText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}
