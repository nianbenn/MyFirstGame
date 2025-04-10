using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ӿ�Ч��
public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;//����

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;//��˸����ʱ��
    [SerializeField] private Material hitMat;//�ܵ��˺�ʱ�Ĳ���
    private Material originalMat;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;//ԭʼ����
    }

    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(flashDuration);//�����л������Դﵽ�ܻ������Ч��
        spriteRenderer.material = originalMat;
    }

    private void RedColorBlink()//��ɫ��˸
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
