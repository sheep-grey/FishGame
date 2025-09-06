using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFishIconUI : MonoBehaviour
{
    [SerializeField] private Transform fishIconTemplate;

    private void Awake()
    {
        fishIconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateVisual();

        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.OverHooking)
        {
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == fishIconTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (FishSO fishSO in HarvestFishManager.Instance.GetHarvestFishSOList())
        {
            Transform fishIconTransform = Instantiate(fishIconTemplate, transform);

            fishIconTransform.gameObject.SetActive(true);

            int fishNum = HarvestFishManager.Instance.GetHarvestFishNumWithSO(fishSO);

            fishIconTransform.GetComponent<FishIconSingleUI>().UpdateVisual(fishSO, fishNum);
        }
    }
}
