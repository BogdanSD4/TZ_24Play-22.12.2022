using System.Collections.Generic;
using UnityEngine;

public class CubePickupTopology
{
    public int CubeCount = 3;
    public CubeController Cube;
    public Transform Parent;
    public CubePickupMode Mode;

    private static List<CubePickupTopology> _topologies = new List<CubePickupTopology>()
    {
        new CubePickupTopology{Mode = CubePickupMode.Right },
        new CubePickupTopology{Mode = CubePickupMode.Left },
        new CubePickupTopology{Mode = CubePickupMode.Middle },
        new CubePickupTopology{Mode = CubePickupMode.RightDiagonal },
        new CubePickupTopology{Mode = CubePickupMode.LeftDiagonal },
        new CubePickupTopology{Mode = CubePickupMode.Random },
    };

    public static void SetTopology(CubeController cube, Transform parent, CubePickupTopology topology = null)
    {
        if(topology == null)
        {
            topology = _topologies[Random.Range(0, _topologies.Count)];
        }

        topology.Cube = cube;
        topology.Parent = parent;
        topology.Spawn();
    }

    private void Spawn()
    {
        var startPointX = 0.0f;
        var startPointY = 0.095f;
        var stepZ = 0.0f;
        
        var shift = 0.0f;
        var random = false;

        var min = 0.0f;
        var max = 0.0f;

        switch (Mode)
        {
            case CubePickupMode.Right:
                startPointX = CubePickup.RIGHT_SPAWN;
                break;
            case CubePickupMode.Left:
                startPointX = CubePickup.LEFT_SPAWN;
                break;
            case CubePickupMode.Middle:
                startPointX = CubePickup.MIDDLE_SPAWN;
                break;
            case CubePickupMode.RightDiagonal:
                startPointX = CubePickup.RIGHT_SPAWN;
                shift = -0.15f;
                break;
            case CubePickupMode.LeftDiagonal:
                startPointX = CubePickup.LEFT_SPAWN;
                shift = 0.15f;
                break;
            case CubePickupMode.Random:
                (min, max) = CubeController._spawnLimits;
                startPointX = Random.Range(min, max);
                random = true;
                break;
        }

        for(int i = 0; i < CubeCount; i++)
        {
            if(i == 0)
            {
                Transform cube = CubeController.Spawn(Cube).transform;
                cube.SetParent(Parent);
                cube.localPosition = new Vector3(startPointX, startPointY, stepZ);
            }
            else
            {
                if (!random)
                {
                    startPointX += shift;
                }
                else
                {
                    startPointX = Random.Range(min, max);
                }
                Transform cube = CubeController.Spawn(Cube).transform;
                cube.SetParent(Parent);
                cube.localPosition = new Vector3(startPointX, startPointY, stepZ);
            }
            stepZ += CubePickup.SPAWN_DISTANCE_Z;
        }
    }
}
[System.Serializable]
public struct CubePickupSettings
{
    public CubeController Cube;
    public CubePickupType Type;
}
public enum CubePickupType
{
    Simple,
}
public enum CubePickupMode
{
    Right,
    Left,
    Middle,
    RightDiagonal,
    LeftDiagonal,
    Random,
}
