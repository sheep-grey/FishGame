using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance
    {
        get; private set;
    }

    public event EventHandler OnStartButtonClick;

    [SerializeField] private Button startButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            OnStartButtonClick?.Invoke(this, EventArgs.Empty);
            Hide();
        });

        Show();
    }

    private void Show()
    {
        transform.gameObject.SetActive(true);
    }

    private void Hide()
    {
        transform.gameObject.SetActive(false);
    }
}
