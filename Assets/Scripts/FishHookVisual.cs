using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer hookMeshRenderer;
    [SerializeField] private MeshRenderer ropeMeshRenderer;

    [SerializeField] private Material hookMaterial;
    [SerializeField] private Material hookInvincibleMaterial;

    [SerializeField] private Material ropeMaterial;
    [SerializeField] private Material ropehookInvincibleMaterial;

    private bool isInvincibleMaterial;

    private void Awake()
    {
        hookMeshRenderer.material = hookMaterial;
        ropeMeshRenderer.material = ropeMaterial;
    }

    private void Update()
    {
        if (FishHook.Instance.GetFishHookInvincibleTimer() > 0)
        {

            if (!isInvincibleMaterial)
            {
                hookMeshRenderer.material = hookInvincibleMaterial;
                ropeMeshRenderer.material = ropehookInvincibleMaterial;
                isInvincibleMaterial = true;
            }
        }
        else
        {
            if (isInvincibleMaterial)
            {
                hookMeshRenderer.material = hookMaterial;
                ropeMeshRenderer.material = ropeMaterial;
                isInvincibleMaterial = false;
            }
        }
    }
}
