using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaBlock : MonoBehaviour, IHasFish
{
    [SerializeField] private SeaBlockSO seaBlockSO;

    private List<Fish> fishList;

    private void Awake()
    {
        fishList = new List<Fish>();
    }

    private void Start()
    {
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.InitData)
        {
            //游戏准备倒计时开始
            if (fishList.Count < seaBlockSO.fishNum)
            {
                //鱼的数量不够
                int need = seaBlockSO.fishNum - fishList.Count;
                for (int i = 0; i < need; i++)
                {
                    int fishKind = Random.Range(0, seaBlockSO.spawnFishSOsList.Count);
                    Transform fishTransform = Instantiate(seaBlockSO.spawnFishSOsList[fishKind].fishPrefab);
                    fishTransform.GetComponent<Fish>().SetFishParent(this);
                }
            }

            foreach (Fish fish in fishList)
            {
                fish.SetSwimingState();
            }
        }
        else if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.OverHooking)
        {
            //到结算界面了
            foreach (Fish fish in fishList)
            {
                fish.SetWaitingState();
            }
        }
    }

    public void AddFish(Fish fish)
    {
        fishList.Add(fish);
    }

    public void RemoveFish(Fish fish)
    {
        fishList.Remove(fish);
    }

    public void SetFish(Fish fish)
    {
        fish.transform.SetParent(GetParentTransform());

        float PosY = Random.Range(-9f, 9f);
        fish.transform.localPosition = new Vector3(0, PosY, 0);

        AddFish(fish);
    }

    public Transform GetParentTransform()
    {
        return transform;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }
}
