using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image fishImage;
    [SerializeField] private TextMeshProUGUI fishNumText;
    [SerializeField] private Image backgroundImage;

    public void UpdateVisual(FishSO fishSO, int fishNum)
    {
        fishImage.sprite = fishSO.fishImage;
        fishNumText.text = "X" + fishNum.ToString();
        backgroundImage.color = fishSO.backgroundColor;
    }
}
