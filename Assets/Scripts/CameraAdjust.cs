using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    private void Awake()
    {
        Camera.main.orthographicSize = Camera.main.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
    }
}
