using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FishSO : ScriptableObject
{
    public Transform fishPrefab;
    public Sprite fishImage;
    public string fishName;
    public int fishValue;
    public float moveSpeed;
    public Color backgroundColor;
}
