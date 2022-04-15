using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity : MonoBehaviour
{
    public float max_hp;
    public float hp;
    public float move_speed;
    public bool is_hit;
    public bool IsDestroy => hp == 0;

    public Action OnHit =null;
    public Action OnDestroy = null;

    [Tooltip("��ƼƼ �ݶ��̴��� (1. �� | 2. ���� 1 | 3. ���� 2 | 4 ���� 3 | ...)")]
    [SerializeField] protected Collider2D[] colliders;

    protected Rigidbody2D rigid;
    protected SpriteRenderer renderer;
    protected abstract void Awake();
    protected abstract void Start();
    protected abstract void Update();
}
