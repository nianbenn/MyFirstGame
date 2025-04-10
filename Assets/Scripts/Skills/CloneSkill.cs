using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);//实例化一个克隆体
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition,cloneDuration,canAttack);//设置克隆体的位置
    }
}
