using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{

    [Header("GaugeObject")]
    [SerializeField] Image HpBack;
    [SerializeField] Image HpGauge;
    [SerializeField] Image HpOutline;

    [SerializeField] Image MpBack;
    [SerializeField] Image MpGauge;
    [SerializeField] Image MpOutline;

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
}
