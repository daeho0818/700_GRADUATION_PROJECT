using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameUIManager : Singleton<InGameUIManager>
{

    [Header("GaugeObject")]
    [SerializeField] Image HpBack;
    [SerializeField] Image HpGauge;
    [SerializeField] Image HpOutline;

    [SerializeField] Image MpBack;
    [SerializeField] Image MpGauge;
    [SerializeField] Image MpOutline;

    [Header("UI Object")]
    [SerializeField] Image black;

    Rect hpContent;
    Rect hpOutline;
    Rect mpContent;
    Rect mpOutline;

    void Start()
    {
        hpContent = HpBack.rectTransform.rect;
        hpOutline = HpOutline.rectTransform.rect;
        mpContent = MpBack.rectTransform.rect;
        mpOutline = MpOutline.rectTransform.rect;

        SetGaugeLength(10, 10);
    }

    void Update()
    {
        
    }

    void SetGaugeLength(int hpLevel, int mpLevel)
    {
        HpBack.rectTransform.rect.Set(hpContent.x, hpContent.y, hpContent.width + hpLevel * 100f, hpContent.height);
        HpGauge.rectTransform.rect.Set(hpContent.x, hpContent.y, hpContent.width + hpLevel * 100f, hpContent.height);
        HpOutline.rectTransform.rect.Set(hpOutline.x, hpOutline.y, hpOutline.width + hpLevel * 100f, hpOutline.height);

        MpBack.rectTransform.rect.Set(mpContent.x, mpContent.y, mpContent.width + mpLevel * 100f, mpContent.height);
        MpGauge.rectTransform.rect.Set(mpContent.x, mpContent.y, mpContent.width + mpLevel * 100f, mpContent.height);
        MpOutline.rectTransform.rect.Set(mpOutline.x, mpOutline.y, mpOutline.width + mpLevel * 100f, mpOutline.height);
    }

    public void SceneMoveFade(Action action)
    {
        StartCoroutine(SceneMoveCoroutine(action));
    }

    IEnumerator SceneMoveCoroutine(Action action = null)
    {
        yield return StartCoroutine(FadeIn(0.5f));
        action?.Invoke();
        yield return StartCoroutine(FadeOut(0.5f));
    }

    IEnumerator FadeOut(float duration)
    {

        float timer = duration;
        Color color = black.color;
        while (timer > 0f)
        {
            color.a = timer / duration;
            black.color = color;
            timer -= Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        black.color = color;
        black.gameObject.SetActive(false);
    }

    IEnumerator FadeIn(float duration)
    {
        black.gameObject.SetActive(true);
        float timer = 0f;
        Color color = black.color;
        while (timer < duration)
        {
            color.a = timer / duration;
            black.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = timer / duration;
        black.color = color;
    }
}
