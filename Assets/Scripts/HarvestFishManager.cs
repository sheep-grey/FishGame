using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class HarvestFishManager : MonoBehaviour, IHasFish
{
    public static HarvestFishManager Instance
    {
        get; private set;
    }

    public event EventHandler OnFishHookedCountMax;

    private List<Fish> harvestFishList;

    private List<FishSO> harvestFishSOList;

    private void Awake()
    {
        Instance = this;

        harvestFishList = new List<Fish>();
        harvestFishSOList = new List<FishSO>();
    }

    private void Start()
    {
        Initialization();

        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
    }

    private void FishGameManager_OnStateChange(object sender, EventArgs e)
    {
        if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.InitData)
        {
            Initialization();
        }
    }

    public void AddFish(Fish fish)
    {
        harvestFishList.Add(fish);
        if (IsHarvestFishMax())
        {
            OnFishHookedCountMax?.Invoke(this, EventArgs.Empty);
        }

        if (!harvestFishSOList.Contains(fish.GetFishSO()))
        {
            //没有添加过这个鱼
            harvestFishSOList.Add(fish.GetFishSO());
        }
    }

    public void RemoveFish(Fish fish)
    {
        harvestFishList.Remove(fish);
    }

    public void SetFish(Fish fish)
    {
        fish.transform.SetParent(GetParentTransform());

        AddFish(fish);
    }

    public Transform GetParentTransform()
    {
        return FishHook.Instance.GetFishStayTransform();
    }

    private void Initialization()
    {
        foreach (Fish fish in harvestFishList)
        {
            fish.DestroySelf();
        }
        harvestFishList.Clear();
        harvestFishSOList.Clear();
    }

    public List<FishSO> GetHarvestFishSOList()
    {
        return harvestFishSOList;
    }

    public int GetHarvestFishNumWithSO(FishSO fishSO)
    {
        int num = 0;
        foreach (Fish fish in harvestFishList)
        {
            if (fish.GetFishSO() == fishSO)
            {
                num++;
            }
        }

        return num;
    }

    public int GetHarvestFishListValue()
    {
        int value = 0;
        foreach (Fish fish in harvestFishList)
        {
            value += fish.GetFishSO().fishValue;
        }

        return value;
    }

    public int GetHarvestFishNum()
    {
        return harvestFishList.Count;
    }

    public bool IsHarvestFishMax()
    {
        return harvestFishList.Count >= GameDataManager.Instance.GetMaxFishHookedCount();
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }
}
