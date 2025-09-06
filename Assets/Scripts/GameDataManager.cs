using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private const string PLAYER_PREFS_GAMEDATA_HASMONEY = "HasMoney";
    private const string PLAYER_PREFS_GAMEDATA_NOWMAXHEALTH = "NowMaxHealth";
    private const string PLAYER_PREFS_GAMEDATA_NOWMAXFISHHOOKEDCOUNT = "NowMaxFishHookedCount";
    private const string PLAYER_PREFS_GAMEDATA_NOWMAXDEEPTH = "NowMaxDeepth";

    public static GameDataManager Instance
    {
        get; private set;
    }

    //private int maxDeepth = 75;
    //private int maxHealth = 3;
    //private int maxFishHookedCount = 7;

    private int hasMoney = 10000;
    private int nowMaxDeepth = 25;//25 50 75
    private int nowMaxHealth = 1;//1 2 3
    private int nowMaxFishHookedCount = 3;//3 5 7


    private void Awake()
    {
        Instance = this;

        hasMoney = PlayerPrefs.GetInt(PLAYER_PREFS_GAMEDATA_HASMONEY, 1000);
        nowMaxHealth = PlayerPrefs.GetInt(PLAYER_PREFS_GAMEDATA_NOWMAXHEALTH, 1);
        nowMaxFishHookedCount = PlayerPrefs.GetInt(PLAYER_PREFS_GAMEDATA_NOWMAXFISHHOOKEDCOUNT, 3);
        nowMaxDeepth = PlayerPrefs.GetInt(PLAYER_PREFS_GAMEDATA_NOWMAXDEEPTH, 25);

    }

    private void Start()
    {
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
        StoreUI.Instance.OnLevelUpAttribute += StoreUI_OnLevelUpAttribute;
    }

    private void StoreUI_OnLevelUpAttribute(object sender, StoreUI.OnLevelUpAttributeEventArgs e)
    {

        if (e.attribute == StoreUI.LevelUpAttribute.MaxHealth)
        {
            ChangeMaxHealth(1);
        }
        else if (e.attribute == StoreUI.LevelUpAttribute.MaxFishNum)
        {
            ChangeMaxFishHookedCount(2);
        }
        else if (e.attribute == StoreUI.LevelUpAttribute.MaxDeepth)
        {
            ChangeMaxDeepth(25);
        }

        ChangeMoney(-e.costMoney);
        StoreUI.Instance.UpdateNowMoneyVisual();
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.OverHooking)
        {
            ChangeMoney(HarvestFishManager.Instance.GetHarvestFishListValue());
        }
    }

    public int GetMoney()
    {
        return hasMoney;
    }

    public int GetMaxDeepth()
    {
        return nowMaxDeepth;
    }

    public int GetMaxHealth()
    {
        return nowMaxHealth;
    }

    public int GetMaxFishHookedCount()
    {
        return nowMaxFishHookedCount;
    }

    private void ChangeMoney(int money)
    {
        hasMoney += money;
        PlayerPrefs.SetInt(PLAYER_PREFS_GAMEDATA_HASMONEY, hasMoney);
        PlayerPrefs.Save();
    }
    private void ChangeMaxDeepth(int value)
    {
        nowMaxDeepth += value;
        PlayerPrefs.SetInt(PLAYER_PREFS_GAMEDATA_NOWMAXDEEPTH, nowMaxDeepth);
        PlayerPrefs.Save();
    }

    private void ChangeMaxHealth(int value)
    {
        nowMaxHealth += value;
        PlayerPrefs.SetInt(PLAYER_PREFS_GAMEDATA_NOWMAXHEALTH, nowMaxHealth);
        PlayerPrefs.Save();
    }

    private void ChangeMaxFishHookedCount(int value)
    {
        nowMaxFishHookedCount += value;
        PlayerPrefs.SetInt(PLAYER_PREFS_GAMEDATA_NOWMAXFISHHOOKEDCOUNT, nowMaxFishHookedCount);
        PlayerPrefs.Save();
    }
}
