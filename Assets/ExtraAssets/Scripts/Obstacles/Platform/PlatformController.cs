using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformController : MonoBehaviour
{
    [SerializeField] private bool _canSpawn = true;
   
    private const int PLATFORM_DISTANCE = 30;
    private const int SET_CURRENT = 10;
    private const int SPWN_DISTANCE = 150;
    private const float SPEED = 10f;

    public static Transform CurrentPlatform;
    
    void Start()
    {
        if (_canSpawn)
        {
            CubePickupSpawn();
            CubeWallSpawn();
        }
    }

    void Update()
    {
        if (GameController.IsStarted)
        {
            Move();
        }
    }

    public virtual void Move()
    {
        if (CurrentPlatform != transform)
        {
            var posZ = transform.localPosition.z;
            if (posZ >= 0 && posZ <= SET_CURRENT)
            {
                CurrentPlatform = transform;
            }
        }

        transform.Translate(0, 0, -SPEED * GameController.GameSpeed * Time.deltaTime);

        if(transform.localPosition.z <= -PLATFORM_DISTANCE)
        {
            Instantiate(GameController.gameController.GetPlatform(), new Vector3(0,0,SPWN_DISTANCE), Quaternion.identity,
                Constans.PlatformPack);
            Destroy(gameObject);
        }
    }

    public virtual void CubePickupSpawn() { }
    public virtual void CubeWallSpawn() { }

    public virtual void ForbidSpawn() => _canSpawn = false;
}
[System.Serializable]
public struct PlatformSettings
{
    public Platform PlatformBody;
    public PlatformType Type;
}
public enum PlatformType
{
    Simple,
}
