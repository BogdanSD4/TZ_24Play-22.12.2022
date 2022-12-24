using System.Collections.Generic;
using UnityEngine;

public class CubeWallTopology
{
    public int[] WallConstructor = new int[25];
    public CubeController Cube;
    public Transform Parent;

    private static List<CubeWallTopology> _topologies = new List<CubeWallTopology>
    {
        new CubeWallTopology{ WallConstructor = new int[25] 
        {
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0,
            1,0,1,0,1,
            1,1,1,1,1,
        } },
        new CubeWallTopology{ WallConstructor = new int[25]
        {
            0,0,0,0,0,
            0,0,0,0,0,
            0,1,0,0,0,
            1,1,1,0,1,
            1,1,1,1,1,
        } },
        new CubeWallTopology{ WallConstructor = new int[25]
        {
            0,0,0,0,0,
            0,0,0,0,0,
            1,0,0,0,1,
            0,1,1,1,0,
            1,1,1,1,1,
        } },
        new CubeWallTopology{ WallConstructor = new int[25]
        {
            0,0,0,0,0,
            0,0,0,0,1,
            0,0,0,0,1,
            1,0,0,1,1,
            1,1,1,1,1,
        } },
        new CubeWallTopology{ WallConstructor = new int[25]
        {
            0,0,0,0,0,
            0,0,1,0,0,
            0,1,1,1,0,
            1,1,1,1,1,
            1,1,1,1,1,
        } },
        new CubeWallTopology{ WallConstructor = new int[25]
        {
            0,0,0,0,0,
            0,0,0,0,0,
            1,1,1,1,1,
            0,0,0,0,0,
            1,1,1,1,1,
        } },
    };

    public static void SetTopology(CubeController cube, Transform parent, CubeWallTopology topology = null)
    {
        if (topology == null)
        {
            topology = _topologies[Random.Range(0, _topologies.Count)];
        }

        if(topology.WallConstructor.Length != 25)
        {
            Debug.Log($"Constructor must have 25 indices: You have ({topology.WallConstructor.Length})");
        }

        topology.Cube = cube;
        topology.Parent = parent;
        topology.Spawn();
    }

    private void Spawn()
    {
        var cubePositionX = CubeWall.STARTPOINT_CUBE_WALL_X;
        var cubePositionY = CubeWall.STARTPOINT_CUBE_WALL_Y;

        var stepX = CubeWall.SPAWNSTEP_CUBE_WALL_X;
        var stepY = CubeWall.SPAWNSTEP_CUBE_WALL_Y;

        for (int i = 0; i < WallConstructor.Length; i++)
        {
            if (WallConstructor[i] == 1)
            {
                Transform cube = CubeController.Spawn(Cube).transform;
                cube.SetParent(Parent);
                cube.localPosition = new Vector3(cubePositionX, cubePositionY, 0);
            }

            if ((i+1) % 5 == 0)
            {
                cubePositionX = CubeWall.STARTPOINT_CUBE_WALL_X;
                cubePositionY += stepY;
            }
            else
            {
                cubePositionX += stepX;
            }
        }
    }
}
[System.Serializable]
public struct CubeWallSettings
{
    public CubeController Cube;
    public CubeWallType Type;
}
public enum CubeWallType
{
    Simple,
}
