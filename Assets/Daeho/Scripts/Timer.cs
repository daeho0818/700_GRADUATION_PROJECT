using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    /// <summary>
    /// 타이머 시작하는 함수
    /// </summary>
    /// <param name="time">기다릴 시간</param>
    /// <param name="action">실행할 내용 (이벤트)</param>
    public void TimerStart(Enemy e, float time, System.Action action)
    {
        processing = true;
        e.StartCoroutine(TimerProcess(time, action));
    }

    bool processing = false;
    public bool Processing() => processing;

    /// <summary>
    /// 타이머 진행
    /// </summary>
    /// <param name="time">기다릴 시간</param>
    /// <param name="action">실행할 내용 (이벤트)</param>
    /// <returns></returns>
    IEnumerator TimerProcess(float time, System.Action action)
    {
        float current_time = 0;

        while (current_time < time)
        {
            current_time += Time.deltaTime;
            yield return null;
        }

        action();
        processing = false;
    }
}