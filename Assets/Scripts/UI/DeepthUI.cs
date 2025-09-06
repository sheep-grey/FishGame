using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeepthUI : MonoBehaviour
{
    private const string POPUP = "Popup";

    [SerializeField] private TextMeshProUGUI deepthText;
    [SerializeField] private TextMeshProUGUI arrivedAreaText;

    private Animator arrivedAreaTextAnimator;

    private void Awake()
    {
        arrivedAreaTextAnimator = arrivedAreaText.transform.GetComponent<Animator>();
    }

    private void Start()
    {
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
        FishGameManager.Instance.OnArrivedArea += FishGameManager_OnArrivedArea;

        Hide();
    }

    private void FishGameManager_OnArrivedArea(object sender, FishGameManager.OnArrivedAreaEventArgs e)
    {
        if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.HookDowning)
        {
            arrivedAreaText.text = e.areaName;
            arrivedAreaTextAnimator.SetTrigger(POPUP);
        }

    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        FishGameManager.GameState gameState = FishGameManager.Instance.GetGameState();
        if (gameState == FishGameManager.GameState.HookDowning)
        {
            Show();
        }
        else if (gameState == FishGameManager.GameState.OverHooking)
        {
            Hide();
        }
    }

    private void Update()
    {
        deepthText.text = Mathf.Ceil(FishGameManager.Instance.GetNowDepth()).ToString() + "M";
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
