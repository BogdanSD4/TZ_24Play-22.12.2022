using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constans : MonoBehaviour
{
    [SerializeField] private Transform _platformPack;
    [SerializeField] private Interface _interface;

    public const string TAG_CUBE_PICKUP = "CubePickup";
    public const string TAG_CUBE_WALL = "CubeWall";
    public const string TAG_UNTAGGED = "Untagged";
    public const float GRAVITY = -8;

    public static Transform PlatformPack;
    public static Interface Interface;
    private void Awake()
    {
        PlatformPack = _platformPack;
        Interface = _interface;
    }
}
