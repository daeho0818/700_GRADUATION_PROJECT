using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Coroutine fall_and_spawn = null;
    /// <summary>
    /// 발판이 제거된 후 일정 시간 뒤 다시 생성하는 함수
    /// </summary>
    /// <param name="wait">발판이 유지되는 시간</param>
    /// <param name="cool_time">발판이 생성되기까지 걸리는 시간</param>
    public void FallAndSpawn(float wait, float cool_time)
    {
        if (fall_and_spawn == null)
            fall_and_spawn = StartCoroutine(_FallAndSpawn(wait, cool_time));
    }
    IEnumerator _FallAndSpawn(float wait, float cool_time)
    {
        yield return new WaitForSeconds(wait);

        // 제거 애니메이션

        gameObject.SetActive(false);

        // 생성 애니메이션

        yield return new WaitForSeconds(cool_time);

        gameObject.SetActive(true);

        fall_and_spawn = null;
    }
}
