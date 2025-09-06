using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    private const string POPUP = "Popup";

    [SerializeField] private TextMeshProUGUI countText;

    private int previousCountNumber;

    private Animator countTextAnimator;

    private void Awake()
    {
        countTextAnimator = countText.transform.GetComponent<Animator>();
    }

    private void Start()
    {
        FishGameManager.Instance.OnStateChange += FishGameManager_OnStateChange;

        Hide();
    }

    private void FishGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (FishGameManager.Instance.GetGameState() == FishGameManager.GameState.CountdownToStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(FishGameManager.Instance.GetCountdownToStartTimer());
        countText.text = countdownNumber.ToString();

        if (previousCountNumber != countdownNumber)
        {
            previousCountNumber = countdownNumber;
            countTextAnimator.SetTrigger(POPUP);
            SoundManager.Instance.PlayCountdownSound(Camera.main.transform.position, 1f);
        }
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
