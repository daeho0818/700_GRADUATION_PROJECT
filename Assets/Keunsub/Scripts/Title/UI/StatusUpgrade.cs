using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatusUpgrade : MonoBehaviour
{

    [Header("Damage Upgrade")]
    public Text damageLevelTxt;
    public Text damageValueTxt;

    [Header("HP Upgrade")]
    public Text hpLevelTxt;
    public Text hpValueTxt;

    [Header("Status")]
    public Text CurToken;
    public Text MessageTxt;

    [Header("Objects")]
    public RectTransform NPC;
    public RectTransform BG;
    public RectTransform TokenBox;
    public RectTransform MessageBox;

    void Start()
    {
    }

    void Update()
    {

    }

    public void UIon()
    {
        NPC.anchoredPosition = new Vector2(1900f, -530f);
        BG.anchoredPosition = new Vector2(-500f, 1300f);
        TokenBox.anchoredPosition = new Vector2(-1800f, -550f);
        MessageBox.anchoredPosition = new Vector2(0f, -1080f);

        NPC.DOAnchorPosX(1100f, 0.5f).SetEase(Ease.OutBack);
        BG.DOAnchorPosY(150f, 0.5f).SetDelay(0.2f).SetEase(Ease.OutBack);
        TokenBox.DOAnchorPosX(-1100f, 0.5f).SetDelay(0.4f).SetEase(Ease.OutBack);
        MessageBox.DOAnchorPosY(-550f, 0.5f).SetDelay(0.6f).SetEase(Ease.OutBack);
    }

    public void UIoff()
    {
        NPC.DOAnchorPosX(1900f, 0.5f).SetEase(Ease.InBack);
        BG.DOAnchorPosY(1300f, 0.5f).SetEase(Ease.InBack);
        TokenBox.DOAnchorPosX(-1800f, 0.5f).SetEase(Ease.InBack);
        MessageBox.DOAnchorPosY(-1080f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            InGameUIManager.Instance.player.isShop = false;
        });
    }
}
