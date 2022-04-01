using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity : MonoBehaviour
{
    public float Hp { get; set; }
    public bool IsDestroy => Hp == 0;

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
