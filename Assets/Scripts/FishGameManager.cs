using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class FishGameManager : MonoBehaviour
{
    public static FishGameManager Instance
    {
        get; private set;
    }

    public enum GameState
    {
        WaitingToCountdown,
        InitData,
        CountdownToStart,
        HookDowning,
        Hooking,
        HookOver,
        OverHooking
    }

    public enum Area
    {
        ShallowSea,
        MiddleSea,
        DeepSea
    }

    public event EventHandler OnStateChange;
    public event EventHandler OnLastOneSecondCountdown;
    public event EventHandler OnAfterTimeOverHooking;
    public event EventHandler<OnArrivedAreaEventArgs> OnArrivedArea;

    public class OnArrivedAreaEventArgs
    {
        public string areaName;
        public Area area;
    }

    private GameState state;
    private float countdownToStartTimer;
    private float countdownToStartTimerMax = 3f;

    private float hookDepthIncreaseSpeed = 4f;
    private float overHookDepthIncreaseSpeed;

    private float afterTimeOverHookingTimer;
    private float afterTimeOverHookingTimerMax = 1.5f;

    private float nowDeepth;

    private bool isGameingCamera;

    private bool isArrivedShallowArea, isArrivedMiddleArea, isArrivedDeepArea;
    private float shallowAreaDepth = 0, middleAreaDepth = 25, deepAreaDepth = 50;

    private void Awake()
    {
        Instance = this;

        state = GameState.WaitingToCountdown;

        Initialization();

        overHookDepthIncreaseSpeed = 3 * hookDepthIncreaseSpeed;
    }

    private void Start()
    {
        HarvestFishManager.Instance.OnFishHookedCountMax += HarvestFishManager_OnFishHookedCountMax;
        FishHook.Instance.OnFishHookHealthZero += FishHook_OnFishHookHealthZero;
        OverHookUI.Instance.OnContinueButtonClick += OverHookUI_OnContinueButtonClick;
        TutorialUI.Instance.OnStartButtonClick += TutorialUI_OnStartButtonClick;
    }

    private void TutorialUI_OnStartButtonClick(object sender, EventArgs e)
    {
        state = GameState.InitData;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void OverHookUI_OnContinueButtonClick(object sender, EventArgs e)
    {
        state = GameState.InitData;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void FishHook_OnFishHookHealthZero(object sender, EventArgs e)
    {
        state = GameState.Hooking;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void HarvestFishManager_OnFishHookedCountMax(object sender, EventArgs e)
    {
        state = GameState.HookOver;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.WaitingToCountdown:
                break;
            case GameState.InitData:
                Initialization();
                state = GameState.CountdownToStart;
                OnStateChange?.Invoke(this, EventArgs.Empty);
                break;
            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;

                if (countdownToStartTimer < 1f && !isGameingCamera)
                {
                    //倒计时最后一秒
                    OnLastOneSecondCountdown?.Invoke(this, EventArgs.Empty);
                }

                if (countdownToStartTimer < 0f)
                {
                    //倒计时结束
                    state = GameState.HookDowning;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.HookDowning:
                nowDeepth += hookDepthIncreaseSpeed * Time.deltaTime;

                if (!isArrivedShallowArea && nowDeepth > shallowAreaDepth)
                {
                    //到浅海区了
                    OnArrivedArea?.Invoke(this, new OnArrivedAreaEventArgs
                    {
                        areaName = "SHALLOW SEA",
                        area = Area.ShallowSea
                    });

                    isArrivedShallowArea = true;
                }
                else if (!isArrivedMiddleArea && nowDeepth > middleAreaDepth)
                {
                    //到中海区了
                    OnArrivedArea?.Invoke(this, new OnArrivedAreaEventArgs
                    {
                        areaName = "MIDDLE SEA",
                        area = Area.MiddleSea
                    });

                    isArrivedMiddleArea = true;
                }
                else if (!isArrivedDeepArea && nowDeepth > deepAreaDepth)
                {
                    //到深海区了
                    OnArrivedArea?.Invoke(this, new OnArrivedAreaEventArgs
                    {
                        areaName = "DEEP SEA",
                        area = Area.DeepSea
                    });

                    isArrivedDeepArea = true;
                }


                if (nowDeepth > GameDataManager.Instance.GetMaxDeepth())
                {
                    //到底了
                    state = GameState.Hooking;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.Hooking:
                nowDeepth -= hookDepthIncreaseSpeed * Time.deltaTime;

                if (isArrivedMiddleArea && nowDeepth < middleAreaDepth)
                {
                    //到浅海区了
                    OnArrivedArea?.Invoke(this, new OnArrivedAreaEventArgs
                    {
                        areaName = "SHALLOW SEA",
                        area = Area.ShallowSea
                    });

                    isArrivedMiddleArea = false;
                }
                else if (isArrivedDeepArea && nowDeepth < deepAreaDepth)
                {
                    //到中海区了
                    OnArrivedArea?.Invoke(this, new OnArrivedAreaEventArgs
                    {
                        areaName = "MIDDLE SEA",
                        area = Area.MiddleSea
                    });

                    isArrivedDeepArea = false;
                }

                if (nowDeepth < 0)
                {
                    //到岸了
                    state = GameState.OverHooking;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.HookOver:
                nowDeepth -= overHookDepthIncreaseSpeed * Time.deltaTime;

                if (isArrivedMiddleArea && nowDeepth < middleAreaDepth)
                {
                    //到浅海区了
                    OnArrivedArea?.Invoke(this, new OnArrivedAreaEventArgs
                    {
                        areaName = "SHALLOW SEA",
                        area = Area.ShallowSea
                    });

                    isArrivedMiddleArea = false;
                }
                else if (isArrivedDeepArea && nowDeepth < deepAreaDepth)
                {
                    //到中海区了
                    OnArrivedArea?.Invoke(this, new OnArrivedAreaEventArgs
                    {
                        areaName = "MIDDLE SEA",
                        area = Area.MiddleSea
                    });

                    isArrivedDeepArea = false;
                }

                if (nowDeepth < 0)
                {
                    //到岸了
                    state = GameState.OverHooking;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.OverHooking:
                afterTimeOverHookingTimer -= Time.deltaTime;

                if (afterTimeOverHookingTimer < 0f)
                {
                    OnAfterTimeOverHooking?.Invoke(this, EventArgs.Empty);

                    state = GameState.WaitingToCountdown;
                }
                break;
        }
    }

    public GameState GetGameState()
    {
        return state;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public float GetNowDepth()
    {
        return nowDeepth;
    }

    private void Initialization()
    {
        countdownToStartTimer = countdownToStartTimerMax;
        afterTimeOverHookingTimer = afterTimeOverHookingTimerMax;
        isArrivedShallowArea = isArrivedMiddleArea = isArrivedDeepArea = false;
        nowDeepth = 0;
        isGameingCamera = false;
    }
}
