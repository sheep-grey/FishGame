using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaBackground : MonoBehaviour
{
    [SerializeField] Color shallowSeaColor;
    [SerializeField] Color middleSeaColor;
    [SerializeField] Color deepSeaColor;

    private Material material;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;

        Initialization();
    }

    private void Start()
    {
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
        FishGameManager.Instance.OnArrivedArea += FishGameMananger_OnArrivedArea;
    }

    private void FishGameMananger_OnArrivedArea(object sender, FishGameManager.OnArrivedAreaEventArgs e)
    {
        if (e.area == FishGameManager.Area.ShallowSea)
        {
            material.color = shallowSeaColor;
        }
        else if (e.area == FishGameManager.Area.MiddleSea)
        {
            material.color = middleSeaColor;
        }
        else if (e.area == FishGameManager.Area.DeepSea)
        {
            material.color = deepSeaColor;
        }
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.InitData)
        {
            Initialization();
        }
    }

    private void Initialization()
    {
        material.color = shallowSeaColor;
    }
}
