using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public static (float, float) _spawnLimits = (-0.4f, 0.4f);

    public static CubeController Spawn(CubeController cube)
    {
        return Instantiate(cube);
    }
}

