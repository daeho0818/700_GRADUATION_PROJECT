using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity : MonoBehaviour
{
    public float max_hp;
    public float hp;
    public float move_speed;
    protected bool movable = true;
    public bool is_hit;
    public bool IsDestroy => hp == 0;

    // damage 매개변수
    public Action<int> OnHit =null;
    public Action OnDestroy = null;

    [Tooltip("엔티티 콜라이더들 (1. 몸 | 2. 공격 1 | 3. 공격 2 | 4 공격 3 | ...)")]
    [SerializeField] protected Collider2D[] colliders;

    protected Rigidbody2D rigid;
    protected new SpriteRenderer renderer;
    protected abstract void Awake();
    protected abstract void Start();
    protected abstract void Update();
}
