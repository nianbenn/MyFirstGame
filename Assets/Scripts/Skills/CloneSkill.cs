using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        GameObject newClone = Instantiate(clonePrefab);//ʵ����һ����¡��
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, offset);//���ÿ�¡���λ��
    }
}
