using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonVisual : MonoBehaviour
{
    private const string HOOKING = "Hooking";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        FishGameManager.Instance.OnLastOneSecondCountdown += FishGameManager_OnLastOneSecondCountdown;
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        FishGameManager.GameState gameState = FishGameManager.Instance.GetGameState();
        if (gameState == FishGameManager.GameState.OverHooking)
        {
            animator.SetBool(HOOKING, false);
        }
    }

    private void FishGameManager_OnLastOneSecondCountdown(object sender, System.EventArgs e)
    {
        animator.SetBool(HOOKING, true);
    }
}
