using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCrack : MonoBehaviour
{
    /// <summary>
    /// �տ��� ���߽�Ű�� �Լ�
    /// </summary>
    public void Explosion()
    {
        transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * 1.5f);
    }
}
