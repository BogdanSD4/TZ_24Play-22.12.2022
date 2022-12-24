using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform _playerPrefab;
    [Space]
    [SerializeField] private List<PlatformSettings> _platforms;
    [SerializeField] private List<CubePickupSettings> _cubePickups;
    [SerializeField] private List<CubeWallSettings> _cubeWalls;
    
    public static float GameSpeed { get; private set; } = 1;
    public static bool IsStarted { get; private set; }
    public static GameController gameController;

    public static Action GameStart;
    public static Action GameEnd;
    public static Action GameReset;

    private Vector3 _playerSpawnPoint = new Vector3(0, 0, 10);
    private void Awake()
    {
        gameController = this;
    }
    void Start()
    {
        SpawnStartPlatform(6);
    }

    void Update()
    {
        
    }



    public static void GameState(bool state)
    {
        if (state)
        {
            GameStart.Invoke();
        }
        else
        {
            GameEnd.Invoke();
        }

        IsStarted = state;
    }

    public void ResetGame()
    {
        ClearLevel();
        GameReset.Invoke();
        ClearGameEvents();

        var player = Instantiate(_playerPrefab, transform);
        player.localPosition = _playerSpawnPoint;

        SpawnStartPlatform(6);
    }

    private void ClearGameEvents()
    {
        GameStart = null;
        GameEnd = null;
        GameReset = null;
        Constans.Interface.SetGameControllerEvents();
    }

    private void ClearLevel()
    {
        var level = Constans.PlatformPack;
        for(int i = 0; i < level.childCount; i++)
        {
            Destroy(level.GetChild(i).gameObject);
        }
    }

    private void SpawnStartPlatform(int count)
    {
        var platformDistance = 30;

        for(int i = 0; i < count; i++)
        {
            var platform = Instantiate(GetPlatform(), Constans.PlatformPack);
            if(i == 0) platform.ForbidSpawn();

            var pos = platform.transform;
            pos.localPosition = new Vector3(0, 0, i * platformDistance);
        }
    }
    public Platform GetPlatform(PlatformType type = PlatformType.Simple)
    {
        foreach(var platform in _platforms)
        {
            if(platform.Type == type)
            {
                return platform.PlatformBody;
            }
        }

        return null;
    }

    public CubeController GetCubeWall(CubeWallType type = CubeWallType.Simple)
    {
        foreach (var cube in _cubeWalls)
        {
            if (cube.Type == type)
            {
                return cube.Cube;
            }
        }

        return null;
    }

    public CubeController GetCubePickup(CubePickupType type = CubePickupType.Simple)
    {
        foreach (var cube in _cubePickups)
        {
            if (cube.Type == type)
            {
                return cube.Cube;
            }
        }

        return null;
    }
}

