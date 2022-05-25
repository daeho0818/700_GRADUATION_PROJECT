using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    /// <summary>
    /// Ÿ�̸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="time">��ٸ� �ð�</param>
    /// <param name="action">������ ���� (�̺�Ʈ)</param>
    public void TimerStart(Enemy e, float time, System.Action action)
    {
        processing = true;
        e.StartCoroutine(TimerProcess(time, action));
    }

    bool processing = false;
    public bool Processing() => processing;

    /// <summary>
    /// Ÿ�̸� ����
    /// </summary>
    /// <param name="time">��ٸ� �ð�</param>
    /// <param name="action">������ ���� (�̺�Ʈ)</param>
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