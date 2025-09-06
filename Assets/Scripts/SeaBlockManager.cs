using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaBlockManager : MonoBehaviour
{
    public static SeaBlockManager Instance
    {
        get; protected set;
    }

    public enum SeaBlockState
    {
        Waiting,
        HookDown,
        Hooking,
        HookOver
    }

    [SerializeField] private Transform shallowSeaBlockPrefab;
    [SerializeField] private Transform middleSeaBlockPrefab;
    [SerializeField] private Transform deepSeaBlockPrefab;

    private SeaBlockState state;

    private List<Transform> seaBlockList;

    private int seaBlockNum = 16;
    private float seaBlockLength = 20f;

    private float hookBlockMoveSpeed = 16f;
    private float overHookBlockMoveSpeed;

    private float originPos;

    private void Awake()
    {
        seaBlockList = new List<Transform>();
        originPos = transform.position.y;

        overHookBlockMoveSpeed = 3f * hookBlockMoveSpeed;
    }

    private void Start()
    {
        SpawnSeaBlock();
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        FishGameManager.GameState gameState = FishGameManager.Instance.GetGameState();
        if (gameState == FishGameManager.GameState.HookDowning)
        {
            state = SeaBlockState.HookDown;
        }
        else if (gameState == FishGameManager.GameState.Hooking)
        {
            state = SeaBlockState.Hooking;
        }
        else if (gameState == FishGameManager.GameState.HookOver)
        {
            state = SeaBlockState.HookOver;
        }
        else if (gameState == FishGameManager.GameState.OverHooking)
        {
            state = SeaBlockState.Waiting;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case SeaBlockState.Waiting:
                transform.position = new Vector3(0f, originPos, 0f);
                break;
            case SeaBlockState.HookDown:
                float moveDistance = hookBlockMoveSpeed * Time.deltaTime;
                transform.position = new Vector3(0f, transform.position.y + moveDistance, 0f);
                break;
            case SeaBlockState.Hooking:
                moveDistance = -hookBlockMoveSpeed * Time.deltaTime;
                transform.position = new Vector3(0f, transform.position.y + moveDistance, 0f);
                break;
            case SeaBlockState.HookOver:
                moveDistance = -overHookBlockMoveSpeed * Time.deltaTime;
                transform.position = new Vector3(0f, transform.position.y + moveDistance, 0f);
                break;
        }
    }



    private void SpawnSeaBlock()
    {
        for (int i = 0; i < seaBlockNum; i++)
        {
            if (i >= 10)
            {
                //深海区
                SpawnSeaBlock(i, deepSeaBlockPrefab);
            }
            else if (i >= 5)
            {
                //中海区
                SpawnSeaBlock(i, middleSeaBlockPrefab);
            }
            else
            {
                //浅海区
                SpawnSeaBlock(i, shallowSeaBlockPrefab);
            }
        }
    }

    private void SpawnSeaBlock(int index, Transform seaBlockPrefab)
    {
        Transform seaBlockTransform = Instantiate(seaBlockPrefab);
        seaBlockTransform.position = new Vector3(0, -index * seaBlockLength, 0);
        seaBlockTransform.SetParent(this.transform);

        seaBlockList.Add(seaBlockTransform);
    }

}
