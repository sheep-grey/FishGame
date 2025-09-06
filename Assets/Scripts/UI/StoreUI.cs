using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    public static StoreUI Instance
    {
        get; private set;
    }

    public enum LevelUpAttribute
    {
        MaxHealth,
        MaxFishNum,
        MaxDeepth
    }

    public event EventHandler OnCloseButtonClick;
    public event EventHandler<OnLevelUpAttributeEventArgs> OnLevelUpAttribute;

    public class OnLevelUpAttributeEventArgs
    {
        public LevelUpAttribute attribute;
        public int costMoney;
    }

    [SerializeField] private TextMeshProUGUI nowMoneyText;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthPriceText;
    [SerializeField] private TextMeshProUGUI maxHealthMaxText;
    [SerializeField] private Button maxHealthLevelUpButton;

    [SerializeField] private TextMeshProUGUI maxFishNumText;
    [SerializeField] private TextMeshProUGUI maxFishNumPriceText;
    [SerializeField] private TextMeshProUGUI maxFishNumMaxText;
    [SerializeField] private Button maxFishNumLevelUpButton;

    [SerializeField] private TextMeshProUGUI maxDeepthText;
    [SerializeField] private TextMeshProUGUI maxDeepthPriceText;
    [SerializeField] private TextMeshProUGUI maxDeepthMaxText;
    [SerializeField] private Button maxDeepthLevelUpButton;

    private int[] maxHealthLevelUpNeedMoney = { 200, 400 };
    private int[] maxFishNumLevelUpNeedMoney = { 300, 600 };
    private int[] maxDeepthLevelUpNeedMoney = { 250, 500 };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateAllVisual();

        closeButton.onClick.AddListener(() =>
        {
            OnCloseButtonClick?.Invoke(this, EventArgs.Empty);
            Hide();
        });

        maxHealthLevelUpButton.onClick.AddListener(() =>
        {
            MaxHealthLevelUp();
        });

        maxFishNumLevelUpButton.onClick.AddListener(() =>
        {
            MaxFishNumLevelUp();
        });

        maxDeepthLevelUpButton.onClick.AddListener(() =>
        {
            MaxDeepthLevelUp();
        });

        OverHookUI.Instance.OnStoreButtonClick += OverHookUI_OnStoreButtonClick;

        Hide();
    }

    private void MaxHealthLevelUp()
    {
        if (GameDataManager.Instance.GetMaxHealth() == 1)
        {
            if (GameDataManager.Instance.GetMoney() >= maxHealthLevelUpNeedMoney[0])
            {
                OnLevelUpAttribute?.Invoke(this, new OnLevelUpAttributeEventArgs
                {
                    attribute = LevelUpAttribute.MaxHealth,
                    costMoney = maxHealthLevelUpNeedMoney[0]
                });
                UpdateMaxHealthVisual();
            }
        }
        else if (GameDataManager.Instance.GetMaxHealth() == 2)
        {
            if (GameDataManager.Instance.GetMoney() >= maxHealthLevelUpNeedMoney[1])
            {
                OnLevelUpAttribute?.Invoke(this, new OnLevelUpAttributeEventArgs
                {
                    attribute = LevelUpAttribute.MaxHealth,
                    costMoney = maxHealthLevelUpNeedMoney[1]
                });
                UpdateMaxHealthVisual();
            }
        }
    }

    private void MaxFishNumLevelUp()
    {
        if (GameDataManager.Instance.GetMaxFishHookedCount() == 3)
        {
            if (GameDataManager.Instance.GetMoney() >= maxFishNumLevelUpNeedMoney[0])
            {
                OnLevelUpAttribute?.Invoke(this, new OnLevelUpAttributeEventArgs
                {
                    attribute = LevelUpAttribute.MaxFishNum,
                    costMoney = maxFishNumLevelUpNeedMoney[0]
                });
                UpdateMaxFishNumVisual();
            }
        }
        else if (GameDataManager.Instance.GetMaxFishHookedCount() == 5)
        {
            if (GameDataManager.Instance.GetMoney() >= maxFishNumLevelUpNeedMoney[1])
            {
                OnLevelUpAttribute?.Invoke(this, new OnLevelUpAttributeEventArgs
                {
                    attribute = LevelUpAttribute.MaxFishNum,
                    costMoney = maxFishNumLevelUpNeedMoney[1]
                });
                UpdateMaxFishNumVisual();
            }
        }
    }

    private void MaxDeepthLevelUp()
    {
        if (GameDataManager.Instance.GetMaxDeepth() == 25)
        {
            if (GameDataManager.Instance.GetMoney() >= maxDeepthLevelUpNeedMoney[0])
            {
                OnLevelUpAttribute?.Invoke(this, new OnLevelUpAttributeEventArgs
                {
                    attribute = LevelUpAttribute.MaxDeepth,
                    costMoney = maxDeepthLevelUpNeedMoney[0]
                });
                UpdateMaxDeepthVisual();
            }
        }
        else if (GameDataManager.Instance.GetMaxDeepth() == 50)
        {
            if (GameDataManager.Instance.GetMoney() >= maxDeepthLevelUpNeedMoney[1])
            {
                OnLevelUpAttribute?.Invoke(this, new OnLevelUpAttributeEventArgs
                {
                    attribute = LevelUpAttribute.MaxDeepth,
                    costMoney = maxDeepthLevelUpNeedMoney[1]
                });
                UpdateMaxDeepthVisual();
            }
        }
    }

    private void OverHookUI_OnStoreButtonClick(object sender, EventArgs e)
    {
        UpdateNowMoneyVisual();
        UpdateMaxDeepthVisual();
        UpdateMaxFishNumVisual();
        UpdateMaxDeepthVisual();

        Show();
    }

    private void UpdateMaxHealthVisual()
    {
        maxHealthMaxText.transform.gameObject.SetActive(false);
        if (GameDataManager.Instance.GetMaxHealth() == 1)
        {
            maxHealthPriceText.text = maxHealthLevelUpNeedMoney[0].ToString();
        }
        else if (GameDataManager.Instance.GetMaxHealth() == 2)
        {
            maxHealthPriceText.text = maxHealthLevelUpNeedMoney[1].ToString();
        }
        else if (GameDataManager.Instance.GetMaxHealth() == 3)
        {
            maxHealthLevelUpButton.transform.gameObject.SetActive(false);
            maxHealthPriceText.transform.gameObject.SetActive(false);
            maxHealthMaxText.transform.gameObject.SetActive(true);
        }

        maxHealthText.text = GameDataManager.Instance.GetMaxHealth().ToString();
    }
    private void UpdateMaxFishNumVisual()
    {
        maxFishNumMaxText.transform.gameObject.SetActive(false);
        if (GameDataManager.Instance.GetMaxFishHookedCount() == 3)
        {
            maxFishNumPriceText.text = maxFishNumLevelUpNeedMoney[0].ToString();
        }
        else if (GameDataManager.Instance.GetMaxFishHookedCount() == 5)
        {
            maxFishNumPriceText.text = maxFishNumLevelUpNeedMoney[1].ToString();
        }
        else if (GameDataManager.Instance.GetMaxFishHookedCount() == 7)
        {
            maxFishNumLevelUpButton.transform.gameObject.SetActive(false);
            maxFishNumPriceText.transform.gameObject.SetActive(false);
            maxFishNumMaxText.transform.gameObject.SetActive(true);
        }

        maxFishNumText.text = GameDataManager.Instance.GetMaxFishHookedCount().ToString();
    }
    private void UpdateMaxDeepthVisual()
    {
        maxDeepthMaxText.transform.gameObject.SetActive(false);
        if (GameDataManager.Instance.GetMaxDeepth() == 25)
        {
            maxDeepthPriceText.text = maxDeepthLevelUpNeedMoney[0].ToString();
        }
        else if (GameDataManager.Instance.GetMaxDeepth() == 50)
        {
            maxDeepthPriceText.text = maxDeepthLevelUpNeedMoney[1].ToString();
        }
        else if (GameDataManager.Instance.GetMaxDeepth() == 75)
        {
            maxDeepthLevelUpButton.transform.gameObject.SetActive(false);
            maxDeepthPriceText.transform.gameObject.SetActive(false);
            maxDeepthMaxText.transform.gameObject.SetActive(true);
        }

        maxDeepthText.text = GameDataManager.Instance.GetMaxDeepth().ToString();
    }

    public void UpdateNowMoneyVisual()
    {
        nowMoneyText.text = "MONEY " + GameDataManager.Instance.GetMoney().ToString();
    }

    private void UpdateAllVisual()
    {
        UpdateNowMoneyVisual();
        UpdateMaxHealthVisual();
        UpdateMaxFishNumVisual();
        UpdateMaxDeepthVisual();
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
