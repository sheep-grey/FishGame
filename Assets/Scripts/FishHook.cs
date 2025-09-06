using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

public class FishHook : MonoBehaviour
{
    public static FishHook Instance
    {
        get; private set;
    }

    public enum FishHookState
    {
        Waiting,
        HookDowning,
        Hooking,
        HookOver,
        OverHooking
    }

    public event EventHandler OnFishHookHealthZero;

    [SerializeField] private Transform[] fishHookVisualTransform;
    [SerializeField] private Transform fishStayTransform;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI fishNumText;

    private float moveSpeed = 3f;

    private FishHookState state = FishHookState.Waiting;
    private new Collider collider;

    private float fishHookInvincibleTimer;
    private float fishHookInvincibleTimerMax = 3f;

    private int nowHealth;

    private float originPos;


    private void Awake()
    {
        Instance = this;
        collider = GetComponent<Collider>();
        originPos = transform.position.y;

        Initalization();
    }

    private void Start()
    {
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;

        Hide();
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        FishGameManager.GameState gameState = FishGameManager.Instance.GetGameState();
        if (gameState == FishGameManager.GameState.CountdownToStart)
        {
            state = FishHookState.Waiting;
        }
        else if (gameState == FishGameManager.GameState.HookDowning)
        {
            state = FishHookState.HookDowning;
            Show();
            healthText.transform.gameObject.SetActive(true);
        }
        else if (gameState == FishGameManager.GameState.Hooking)
        {
            healthText.transform.gameObject.SetActive(false);
            fishNumText.transform.gameObject.SetActive(true);
            state = FishHookState.Hooking;
        }
        else if (gameState == FishGameManager.GameState.HookOver)
        {
            state = FishHookState.HookOver;
        }
        else if (gameState == FishGameManager.GameState.OverHooking)
        {
            state = FishHookState.OverHooking;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Fish>(out Fish fish))
        {
            if (state == FishHookState.HookDowning)
            {
                //Óã¹³ÏÂ½µ
                nowHealth--;
                healthText.text = "HP " + nowHealth.ToString();
                SoundManager.Instance.PlayHurtSound(Camera.main.transform.position, 1f);
                if (nowHealth == 0)
                {
                    OnFishHookHealthZero?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (state == FishHookState.Hooking)
            {
                //µöÓã
                fish.SetHookedState();
                fish.SetFishParent(HarvestFishManager.Instance);

                SoundManager.Instance.PlayGetFishSound(Camera.main.transform.position, 1f);

                fishNumText.text = "FISH " + HarvestFishManager.Instance.GetHarvestFishNum().ToString();
                if (HarvestFishManager.Instance.IsHarvestFishMax())
                {
                    fishNumText.text = "FISH MAX";
                }
            }

        }
    }

    private void Update()
    {
        switch (state)
        {
            case FishHookState.Waiting:
                Initalization();
                break;
            case FishHookState.HookDowning:
                DownMoveHandler();

                if (fishHookInvincibleTimer > 0)
                {
                    //ÎÞµÐ×´Ì¬
                    fishHookInvincibleTimer -= Time.deltaTime;
                    collider.enabled = false;
                }
                else
                {
                    collider.enabled = true;
                }
                break;
            case FishHookState.Hooking:
                HookingMoveHandler();
                break;
            case FishHookState.HookOver:
                collider.enabled = false;
                break;
            case FishHookState.OverHooking:
                Vector3 targetPosition = new Vector3(transform.position.x, originPos, 0);
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                if (Mathf.Abs(transform.position.y - originPos) < 0.5f)
                {
                    Hide();
                }
                break;
        }
    }

    private void DownMoveHandler()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float posX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(mousePosition).x, -5, 5);
        float posY = Mathf.Clamp(Camera.main.ScreenToWorldPoint(mousePosition).y, 4f, 5f);
        Vector3 targetPosition = new Vector3(posX, posY, 0);

        Touchscreen touch = Touchscreen.current;

        if (touch == null)
        {
            //Êó±ê
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            //´¥ÃþÆÁ
            TouchControl firstFinger = touch.touches[0];
            if (firstFinger.press.wasPressedThisFrame || firstFinger.press.isPressed)
            {
                Vector2 touchPosition = firstFinger.position.ReadValue();
                posX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(touchPosition).x, -5, 5);
                posY = Mathf.Clamp(Camera.main.ScreenToWorldPoint(touchPosition).y, 4f, 5f);
                targetPosition = new Vector3(posX, posY, 0);

                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }



    }

    private void HookingMoveHandler()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float posX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(mousePosition).x, -5, 5);
        float posY = Mathf.Clamp(Camera.main.ScreenToWorldPoint(mousePosition).y, -5f, -4f);
        Vector3 targetPosition = new Vector3(posX, posY, 0);

        Touchscreen touch = Touchscreen.current;

        if (touch == null)
        {
            //Êó±ê
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            //´¥ÃþÆÁ
            TouchControl firstFinger = touch.touches[0];
            if (firstFinger.press.wasPressedThisFrame || firstFinger.press.isPressed)
            {
                Vector2 touchPosition = firstFinger.position.ReadValue();
                posX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(touchPosition).x, -5, 5);
                posY = Mathf.Clamp(Camera.main.ScreenToWorldPoint(touchPosition).y, -5f, -4f);
                targetPosition = new Vector3(posX, posY, 0);

                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    public Transform GetFishStayTransform()
    {
        return fishStayTransform;
    }

    private void Hide()
    {
        healthText.transform.gameObject.SetActive(false);
        fishNumText.transform.gameObject.SetActive(false);

        foreach (Transform transform in fishHookVisualTransform)
        {
            transform.gameObject.SetActive(false);
        }
    }

    private void Show()
    {
        foreach (Transform transform in fishHookVisualTransform)
        {
            transform.gameObject.SetActive(true);
        }
    }

    public FishHookState GetFishHookState()
    {
        return state;
    }

    public float GetFishHookInvincibleTimer()
    {
        return fishHookInvincibleTimer;
    }

    private void Initalization()
    {
        fishHookInvincibleTimer = fishHookInvincibleTimerMax;
        nowHealth = GameDataManager.Instance.GetMaxHealth();
        healthText.text = "HP " + nowHealth.ToString();
        fishNumText.text = "FISH 0";
    }
}
