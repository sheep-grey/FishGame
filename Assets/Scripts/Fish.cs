using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public enum FishState
    {
        Wating,
        Swiming,
        Hooked
    }

    [SerializeField] private FishSO fishSO;
    [SerializeField] private ParticleSystem moveParticle;

    private new Collider collider;
    private float targetPosX;

    private FishState state = FishState.Wating;

    private IHasFish fishParent;

    private float hookedPosResetSpeed = 5f;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        ParticleSystem.MainModule mainModule = moveParticle.main;
        mainModule.customSimulationSpace = fishParent.GetTransform();
    }

    private void Update()
    {
        switch (state)
        {
            case FishState.Wating:
                break;
            case FishState.Swiming:
                Move();
                break;
            case FishState.Hooked:
                collider.enabled = false;
                moveParticle.transform.gameObject.SetActive(false);
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, 0f), Time.deltaTime * hookedPosResetSpeed);
                break;
        }
    }

    private void Move()
    {
        AdjustRotate();

        if (Mathf.Abs(transform.localPosition.x - targetPosX) > 0.3f)
        {
            //没游到指定位置
            Vector3 targetPos = new Vector3(targetPosX, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * fishSO.moveSpeed);
        }
        else
        {
            //游到指定位置了
            targetPosX = Random.Range(-5f, 5f);

            AdjustRotate();
        }
    }

    private void AdjustRotate()
    {
        if (targetPosX < transform.localPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public FishSO GetFishSO()
    {
        return fishSO;
    }

    public void SetHookedState()
    {
        state = FishState.Hooked;
    }
    public void SetSwimingState()
    {
        state = FishState.Swiming;
    }
    public void SetWaitingState()
    {
        state = FishState.Wating;
    }

    public void SetFishParent(IHasFish fishParent)
    {
        if (this.fishParent != null)
        {
            this.fishParent.RemoveFish(this);
        }

        this.fishParent = fishParent;
        fishParent.SetFish(this);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
