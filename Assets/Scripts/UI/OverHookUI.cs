using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverHookUI : MonoBehaviour
{
    public static OverHookUI Instance
    {
        get; private set;
    }

    public event EventHandler OnContinueButtonClick;
    public event EventHandler OnStoreButtonClick;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI addMoneyText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            OnContinueButtonClick?.Invoke(this, EventArgs.Empty);
            Hide();
        });

        storeButton.onClick.AddListener(() =>
        {
            OnStoreButtonClick?.Invoke(this, EventArgs.Empty);
            Hide();
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });


        FishGameManager.Instance.OnAfterTimeOverHooking += FishGameManager_OnAfterTimeOverHooking;
        StoreUI.Instance.OnCloseButtonClick += StoreUI_OnCloseButtonClick;

        Hide();
    }

    private void StoreUI_OnCloseButtonClick(object sender, EventArgs e)
    {
        Show();
    }

    private void FishGameManager_OnAfterTimeOverHooking(object sender, System.EventArgs e)
    {
        addMoneyText.text = "MONEY " + HarvestFishManager.Instance.GetHarvestFishListValue().ToString();

        Show();
    }

    private void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    private void Show()
    {
        transform.gameObject.SetActive(true);
    }
}
