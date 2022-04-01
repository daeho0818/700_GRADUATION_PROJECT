using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Coroutine fall_and_spawn = null;
    /// <summary>
    /// ������ ���ŵ� �� ���� �ð� �� �ٽ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="wait">������ �����Ǵ� �ð�</param>
    /// <param name="cool_time">������ �����Ǳ���� �ɸ��� �ð�</param>
    public void FallAndSpawn(float wait, float cool_time)
    {
        if (fall_and_spawn == null)
            fall_and_spawn = StartCoroutine(_FallAndSpawn(wait, cool_time));
    }
    IEnumerator _FallAndSpawn(float wait, float cool_time)
    {
        yield return new WaitForSeconds(wait);

        // ���� �ִϸ��̼�

        gameObject.SetActive(false);

        // ���� �ִϸ��̼�

        yield return new WaitForSeconds(cool_time);

        gameObject.SetActive(true);

        fall_and_spawn = null;
    }
}
