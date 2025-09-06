using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookStickVisual : MonoBehaviour
{
    [SerializeField] private Transform hook;
    [SerializeField] private Transform stick;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, hook.position);
        lineRenderer.SetPosition(1, stick.position);
    }
}
