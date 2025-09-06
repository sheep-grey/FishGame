using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance
    {
        get; private set;
    }

    [SerializeField] private CinemachineVirtualCamera waitingCamera;

    [SerializeField] private CinemachineVirtualCamera[] inSceneCameras;

    private float designCameraViewWidth = 11.25f;


    private void Awake()
    {
        Instance = this;

        AddjustCameraToScreen();
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
            waitingCamera.Priority = 10;
        }
    }

    private void FishGameManager_OnLastOneSecondCountdown(object sender, System.EventArgs e)
    {
        waitingCamera.Priority = 0;
    }

    private void AddjustCameraToScreen()
    {
        float nowScreenWidth = Screen.width;
        float nowScreenHeight = Screen.height;
        float nowNeedHidthTarget = (nowScreenWidth * 2 * Camera.main.orthographicSize) / designCameraViewWidth;

        float needRatio = (nowScreenHeight - nowNeedHidthTarget) / nowScreenHeight;

        float rectH = 1f - needRatio;
        float rectY = needRatio / 2f;

        Camera.main.rect = new Rect(0, rectY, 1, rectH);
    }
}
