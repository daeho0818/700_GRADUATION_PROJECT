using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Tooltip("엔티티 콜라이더들 (1. 몸 | 2. 공격 1 | 3. 공격 2 | 4 공격 3 | ...)")]
    [SerializeField] protected Collider2D[] colliders;
    protected Rigidbody2D rigid;
    protected abstract void Awake();
    protected abstract void Start();
    protected abstract void Update();
}
