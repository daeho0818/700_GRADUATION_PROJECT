using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : Singleton<StatusManager>
{
    [Header("Status")]
    public float Hp;
    public float Mp;
    public float MoveSpeed;
    public float JumpForce;
    public float Defence;
    public float Damage;
    public float SkillDamage = 1;
    public float AttackSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
