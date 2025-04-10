using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//视觉效果
public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;//线条

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;//闪烁持续时间
    [SerializeField] private Material hitMat;//受到伤害时的材质
    private Material originalMat;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;//原始材质
    }

    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(flashDuration);//来回切换材质以达到受击闪光的效果
        spriteRenderer.material = originalMat;
    }

    private void RedColorBlink()//红色闪烁
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
